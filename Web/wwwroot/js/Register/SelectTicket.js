let totalprice;
let totalamount;

const ticketCards = document.querySelectorAll(".ticketCard"); //選得到
const plusButtons = document.querySelectorAll(".fa-plus"); //選得到
const minusButtons = document.querySelectorAll(".fa-minus"); //選得到
const totalCount = document.querySelector("span.totalCount"); //選得到
const totalPrice = document.querySelector("span.totalPrice"); //選得到

const signUpBtn = document.querySelector(".sign-up");

let tickets = [];

//let ticketType = [
//    { id: 1, ticketName: 'General Admission', stock: 100, maxPurchase: 5 },
//    { id: 2, ticketName: 'VIP', stock: 50, maxPurchase: 2 },
//];

console.log(ticketType);

// 用於丟進API的全域物件
let selectTicketApiPostJson;

selectTicketApiPostJson = {
    eventId: 1,
    tickets: [
        {
            ticketTypeId: 1,
            count: 100,
        },
        {
            ticketTypeId: 2,
            count: 50,
        },
        // ...
    ],
};
// 格式如上，照著變更填入資料就能用了

document.addEventListener("DOMContentLoaded", function () {
    ChangeTicketStyle();
    initializeTickets();
    setupPlusButtons();
    setupMinusButtons();
    updateTotalPrice();
});

// 初始化每個票種的數量和價格
function initializeTickets() {
    ticketCards.forEach(function (ticketCard, index) {
        let ticketName = ticketCard.querySelector(".ticketName .name").textContent;
        //取得票券的名稱
        let priceText = ticketCard.querySelector(".ticketName .unitprice").textContent;
        let price = parseFloat(priceText.replace("NT$", "").replace(",", "")); // 將價格轉換為數字
        //取得票券價格單價

        //var ticketId = ticketName; // 創建一個唯一的標識
        //ticketCard.setAttribute('data-name', ticketId); // 設置標識
        //// tickets[ticketId] = { name: ticketName, count: 0, price: price };
        //tickets[ticketId] = { ticketName: ticketId ,count: 0, price: price, subTotal: 0 };

        // 從 ticketType 陣列中找到對應的票券類型資訊
        let ticketTypeInfo = ticketType.find((t) => t.ticketName === ticketName);
        let ticket = {
            ticketTypeId: ticketTypeInfo.id,
            ticketName: ticketName,
            eventId: ticketTypeInfo.eventId,
            count: 0,
            price: price,
            subTotal: 0,
            validTime: 0,
            stock: ticketTypeInfo.stock, // 新增庫存
            maxPurchase: ticketTypeInfo.maxPurchase, // 新增最大購買量
        };
        //var ticket = {
        //    ticketid: ticketType.id,
        //    ticketName: ticketType.ticketName,
        //    stock: ticketType.stock,
        //    maxPurchase: ticketType.maxPurchase,

        //};
        tickets.push(ticket);
        updateTicketCountDisplay(ticketCard); // 初始化票數顯示
        SaveTicketsToselectTicketApiPostJson();
    });
}

// 更新票券數量和總價的函數
function updateTotalPrice() {
    let totalTickets = 0;
    let totalPriceValue = 0;
    //for (var ticketId in tickets) {
    //    var count = tickets[ticketId].count;
    //    var subTotal = count * tickets[ticketId].price;
    //    tickets[ticketId].subTotal = subTotal;
    //    totalTickets += count;
    //    totalPriceValue += subTotal;
    //}
    //totalCount.textContent = totalTickets;
    //totalPrice.textContent = 'NT$ ' + totalPriceValue.toFixed(2);

    tickets.forEach(function (ticket) {
        let subTotal = ticket.count * ticket.price;
        ticket.subTotal = subTotal;
        totalTickets += ticket.count;
        totalPriceValue += subTotal;
    });
    totalCount.textContent = totalTickets;
    totalPrice.textContent = "NT$ " + totalPriceValue.toFixed(2);

    SaveTicketsToSession();
    SaveTicketsToselectTicketApiPostJson();
}

// 更新票數顯示的函數
function updateTicketCountDisplay(ticketCard) {
    //var ticketId = ticketCard.getAttribute('data-name');
    //var ticketCountSpan = ticketCard.querySelector('.ticketName .unitprice');
    //ticketCountSpan.textContent = 'NT$ ' + tickets[ticketId].price + ' - ' + tickets[ticketId].count + '張';

    let ticketName = ticketCard.querySelector(".ticketName .name").textContent;
    let ticket = tickets.find((t) => t.ticketName === ticketName);
    let ticketCountSpan = ticketCard.querySelector(".ticketName .unitprice");
    ticketCountSpan.textContent = "NT$ " + ticket.price + " - " + ticket.count + "張";
}

// 處理點擊.fa-plus的事件
function setupPlusButtons() {
    document.querySelectorAll(".fa-plus").forEach(function (plusIcon) {
        plusIcon.addEventListener("click", function () {
            // event.stopPropagation(); // 事件冒泡
            let ticketCard = this.closest('.ticketCard');
            let ticketId = ticketCard.getAttribute('data-name');
            let rightContainer = this.closest('.right');
            let plusIconSpan = rightContainer.querySelector('span:first-child');
            let minusIcon = rightContainer.querySelector('.fa-minus');
            let ticketName = ticketCard.querySelector('.ticketName .name').textContent;
            let ticket = tickets.find(t => t.ticketName === ticketName);
            let ticketTypeInfo = ticketType.find(t => t.ticketName === ticketName);
            //檢查主辦方是否有設定最大購買量
            let purchaseLimit;
            if(ticketTypeInfo.maxPurchase === 0){
                purchaseLimit =ticketTypeInfo.stock;
            }
            else {
                purchaseLimit = Math.min(ticketTypeInfo.stock, ticketTypeInfo.maxPurchase);
            }
            
            if (ticket.count < purchaseLimit) {
                ticket.count++;
                updateTotalPrice();
                updateTicketCountDisplay(ticketCard);
                // 更新票券的庫存量
                // ticketTypeInfo.stock--;
                // 如果選擇量等於庫存量，則禁用加號按鈕，但當用戶點擊減號，選擇量再次小於庫存量時要再將加號按鈕打開
                if (tickets.count >= ticketType.stock) {
                    this.style.pointerEvents = "none";
                    plusIconSpan.style.backgroundColor = "#f3f4f436";
                }else{
                    this.style.pointerEvents="auto";
                    plusIconSpan.style.backgroundColor = "#2ea4f2";
                }
                // 顯示減號按鈕
                minusIcon.style.display = "inline";
                rightContainer.classList.add("active");
                ticketCard.classList.add("active");
                signUpBtn.disabled = false;
            } else {
                // 票券數量達到最大購買量，改變按鈕樣式並禁用
                this.style.pointerEvents = "none";
                plusIconSpan.style.backgroundColor = "#f3f4f436";
            }
        });
    });
}

// 處理點擊.fa-minus的事件
function setupMinusButtons() {
    document.querySelectorAll(".fa-minus").forEach(function (minusIcon) {
        minusIcon.addEventListener("click", function () {
            event.stopPropagation(); // 事件冒泡
            let ticketCard = this.closest(".ticketCard");
            let ticketId = ticketCard.getAttribute("data-name");
            let rightContainer = this.closest(".right");
            let plusIconSpan = rightContainer.querySelector("span:first-child");
            let plusIcon = rightContainer.querySelector(".fa-plus");
            let ticketName = ticketCard.querySelector(".ticketName .name").textContent;
            let ticket = tickets.find((t) => t.ticketName === ticketName);
            let ticketTypeInfo = ticketType.find((t) => t.ticketName === ticketName);
            if (ticket.count > 0) {
                ticket.count--;
                updateTotalPrice();
                updateTicketCountDisplay(ticketCard);
                if (ticket.count === 0) {
                    minusIcon.style.display = 'none';
                    plusIcon.style.display = 'inline';
                    rightContainer.classList.remove('active'); 
                    ticketCard.classList.remove('active'); 
                    
                    //檢查tickets裡是不是還有選擇的票券-> count > 0
                    let isNullTickets = tickets.some(ticket=>ticket.count>0);
                    if(!isNullTickets){
                        signUpBtn.disabled = true;
                    }
                    
                }
            }
            // 當票數小於最大購買量，重置 .fa-plus
            if (ticket.count < ticketTypeInfo.stock) {
                plusIcon.style.pointerEvents = "auto"; // 啟用點擊事件
                plusIconSpan.style.backgroundColor = "";
            }
        });
    });
}

function SaveTicketsToSession() {
    let ticketsJSON = JSON.stringify(tickets);
    sessionStorage.setItem("tickets", ticketsJSON);
}

function ChangeTicketStyle() {
    ticketCards.forEach(function (ticketCard) {
        // 在每張票卡內部選擇 "更多資訊" 文字
        let ticketDetailToggle = ticketCard.querySelector(".ticketDetailToggle");

        // 為 "更多資訊" 文字添加點擊事件監聽器
        ticketDetailToggle.addEventListener("click", function () {
            // 從當前的 ticketCard 元素開始，找到對應的 .showTicketDetail-card
            let showTicketDetailCard = ticketCard.nextElementSibling;

            // 切換詳細資訊卡片的顯示狀態
            showTicketDetailCard.classList.toggle("active");
        });
    });
}

function SaveTicketsToselectTicketApiPostJson() {
    let storedTickets = JSON.parse(sessionStorage.getItem(tickets)) || [];
    let eventId = tickets[0].eventId;
    let selectedTickets = tickets
        .filter((ticket) => ticket.count > 0)
        .map((ticket) => {
            return {
                ticketTypeId: ticket.ticketTypeId,
                count: ticket.count,
            };
        });
    selectTicketApiPostJson.eventId = eventId;
    selectTicketApiPostJson.tickets = selectedTickets;
}

//以上by Tino

// 以下by  Adam

const submit_btn = document.getElementById("submit");

submit_btn.addEventListener("click", function () {
    let cartId = CallSelectTicketApi();
    // window.location.assign(`/Register/FillForm/?cartId=${cartId}`);
});

async function CallSelectTicketApi() {
    let requestBoby = JSON.stringify(selectTicketApiPostJson);
    try {
        let options = {
            method: "POST",
            headers: {
                "Content-Type": "application/json",
            },
            body: requestBoby,
        };
        let response = await fetch("/api/Register/SelectTicket", options);
        let data = await response.json();
        console.log(data.cartId);
        window.location.assign(`/Register/FillForm/?cartId=${data.cartId}`);
        // return data.cartId;
    } catch (err) {}
}
