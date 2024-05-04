const paymentButtons = document.querySelectorAll(".payment");
const nextButton = document.querySelector(".next-btn");

const TicketNames = document.querySelectorAll(".ticket-name"); //Ticket-name會等於localStorage的key
const counts = document.querySelectorAll(".order-detail-description .counts");
const unitPrices = document.querySelectorAll(".order-detail-description .price");
const subTotals = document.querySelectorAll(".order-detail-description .subtotal");

const totalPrice = document.querySelector(".total-price .price");
const cardLeftPrice = document.querySelector(".card-left .price");
const nextbtnPrice = document.querySelector(".next-btn span:nth-child(2)");

const Time = document.querySelector(".eventInfo p.eventTime");
let tickets;

// 判斷目前選擇的付款方式，由按鈕的id賦值，LinePay、Ecpay
let selectedPaymentType;

let detailTemplate = (ticketKey) => `
    <div class="order-detail-description">
        <span class="event">
            <div class="ticket-name">${tickets[ticketKey].ticketName}</div>
            <div class="ticket-time">${Time.textContent}</div>
        </span>
        <span class="counts">${tickets[ticketKey].count} 張</span>
        <span class="price">NT$ ${tickets[ticketKey].price}</span>
        <span class="subtotal">NT$ ${tickets[ticketKey].subTotal}</span>
    </div>`;

window.onload = function () {
    TicketDtetailFromSession();
    // RenderingOrderDetail();
    if (orderId != 0) {
        Swal.fire({
            icon: 'warning',
            title: '付款失敗',
            text: '成立訂單出現異常，若多次嘗試後仍失敗，請聯繫服務人員',
            showConfirmButton: true,
            heightAuto: false,
            customClass: {
                title: 'S2_title_class',
                content: 'S2_content_class',
            }
        });
    }
};

//從sessionStorage提取購買票券資訊
function TicketDtetailFromSession() {
    let ticketsJson = sessionStorage.getItem("tickets");
    if (ticketsJson) {
        tickets = JSON.parse(ticketsJson);
    }
}

//將S渲染到訂購明細上
function RenderingOrderDetail() {
    let index = 0;
    let total = 0; // 總金額
    let orderDetailsHTML = "";

    for (let ticketKey in tickets) {
        if (tickets[ticketKey].count > 0) {
            orderDetailsHTML += detailTemplate(ticketKey);
            total += tickets[ticketKey].subTotal;
        }
    }
    const orderDetailsContainer = document.querySelector(".order-details-container");
    orderDetailsContainer.innerHTML = orderDetailsHTML;

    //顯示總金額區
    totalPrice.textContent = "NT$ " + total.toFixed(2);
    cardLeftPrice.textContent = "NT$ " + total.toFixed(2);
    nextbtnPrice.textContent = "(NT$ " + total.toFixed(2) + ")";
}

//function sendTicketsToServer() {
//    var ticketsJson = sessionStorage.getItem("tickets");
//    console.log("事件觸發");
//    console.log(ticketsJson); //字串有東西
//    if (ticketsJson) {
//        let tickets = JSON.parse(ticketsJson);
//        let t = { Tickets: tickets };
//        console.log(t);
//        let ticketsForBackend = tickets.map(function (ticket) {
//            return {
//                ticketName: ticket.ticketName,
//                count: ticket.count,
//                price: ticket.price,
//                subTotal: ticket.subTotal,
//                validTime: ticket.validTime
//            };
//        });
//        alert(t);
//        //console.log(tickets); //陣列包物件有東西
//        //console.log(ticketsForBackend); //陣列包物件有東西
//        $.ajax({
//            url: 'https://localhost:7138/Register/Complete',
//            type: 'POST',
//            contentType: 'application/json; charset=utf-8',
//            data: JSON.stringify(t),
//            dataType: 'json',
//            success: function (response) {
//                console.log('打中:', response);
//            },
//            error: function ( error) {
//                console.error('Error sending data:', error);
//            }
//        });
//    }
//}

//document.querySelector('.button-group .next-btn').addEventListener('click', sendTicketsToServer);

//付款方式選擇
paymentButtons.forEach((button) => {
    button.addEventListener("click", function () {
        const isSelected = this.classList.contains("selected");
        
        paymentButtons.forEach((btn) => btn.classList.remove("selected"));
        if(!isSelected){
            this.classList.add("selected");
            selectedPaymentType = this.id; // 以id賦值
        }else{
            selectedPaymentType=null;
        }
        updateNextButtonState();
    });
});

function updateNextButtonState() {
    const selectedPayment = document.querySelector(".payment.selected");
    nextButton.disabled = !selectedPayment;
}

//重新報名跳轉至首頁頁
document.querySelector(".restart-btn").onclick = function () {
    window.location.assign(`/Home/Index`);
};

// nextButton.addEventListener("click", () => {
//     window.location.assign(`/Register/Complete?EventId=${eventId}`);
// });

document.querySelector(".step-container:first-child .step").onclick = function () {
    window.location.assign(`/Register/Index?EventId=${eventId}`);
};

document.querySelector(".step-container:nth-child(2) .step").onclick = function () {
    window.location.assign(`/Register/FillForm?EventId=${eventId}`);
};

// by Adam
nextButton.addEventListener("click", function () {
    switch (selectedPaymentType) {
        case "Ecpay":
            CallCreateOrderApi().then((data) => {
                if (data.isSuccess) {
                    window.location.assign(`/Register/DirectToECPay/?orderId=${data.result}`);
                } else {
                    // TODO 處理彈窗，不要使用alert
                    alert("成立訂單出現異常，若多次嘗試後仍失敗，請聯繫服務人員");
                }
            });
            break;
        case "LinePay":
            // todo
            break;
    }
});

async function CallCreateOrderApi() {
    let result = {
        isSuccess: false,
        message: "",
        result: -1,
    };
    let requestBoby = JSON.stringify({ cartId: cartId, orderId: orderId });
    try {
        let options = {
            method: "POST",
            headers: {
                "Content-Type": "application/json",
            },
            body: requestBoby,
        };
        let response = await fetch("/api/Register/CreateOrder", options);
        let data = await response.json();
        console.log(data);
        result = data;
    } catch (err) {
        result.message = `前端出現錯誤，錯誤訊息：${err}`;
        console.error(err);
    }
    return result;
}
