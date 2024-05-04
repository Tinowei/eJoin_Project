let lastResult, countResults = 0;
const ticketInput = document.querySelector('.ticketInput');
const verifyButton = document.querySelector('.verifyButton');
let verifiedTicketCounts=0;


function onScanSuccess(decodedText, decodedResult) {
    if (decodedText !== lastResult) {
        ++countResults;
        lastResult = decodedText;
        console.log(`Scan result ${decodedText}`, decodedResult);
        ticketInput.value=decodedText;
    }
}
var html5QrcodeScanner = new Html5QrcodeScanner("qr-reader", { fps: 10, qrbox: 250 });
html5QrcodeScanner.render(onScanSuccess);


verifyButton.addEventListener('click', function () {
    let currentInputValue = ticketInput.value;
    CallVerifyTicketApi(currentInputValue);
});

function showTicketUsedAlert(){
    Swal.fire({
        icon: 'warning',
        title: '【該票券已被使用過】',
        text: '請至會員中心中「我的票券」重新兌票',
        showConfirmButton: true,
        confirmButtonText:'確認',
        heightAuto: false,
        customClass: {
            title: 'S2_title_class',
            content: 'S2_content_class',
        }
    });
}

function showSuccessAlert(data){
    Swal.fire({
        icon: 'success',
        title: '票券核銷成功',
        text: `${verifiedTicketCounts}張票券已核銷!`,
        showConfirmButton: true,
        confirmButtonText:'確認',
        heightAuto: false,
        customClass: {
            title: 'S2_title_class',
            content: 'S2_content_class',
        }
    });
}

function showUnAuthorizedAlert(){
    Swal.fire({
        icon: 'warning',
        // title: '【你不是該票券的主辦方】',
        text: '你不是主辦方，沒有權限可核銷票券!',
        showConfirmButton: true,
        confirmButtonText:'確認',
        // timer: 3000,
        heightAuto: false,
        customClass: {
            title: 'S2_title_class',
            content: 'S2_content_class',
        }
    });
        // .then(function (){
        // setTimeout(()=>{window.location.assign('/Login');},0)
    // });
}

async function CallVerifyTicketApi(ticketNumbers){
    //先存票券編號（可能是複數）
    let params = {ticketNumbers:ticketNumbers};
    try{
        let queryParams = new URLSearchParams(params);
        let options={method:"POST"};
        let response = await fetch(`/api/ticket/ValidateTickets?${queryParams}`,options);

        if(!response.ok){
            let data = await response.json();
            // 根據不同的錯誤狀態碼處理錯誤訊息
            switch (response.status) {
                case 401: // Unauthorized
                    console.log(data.message);
                    showUnAuthorizedAlert();
                    break;
                case 400: // Bad Request
                    // 對於400錯誤，可能是因為票券編號為空或者票券已經被核銷
                    console.log(data.message);
                    showTicketUsedAlert();
                    break;
                default: // 其他錯誤
                    throw new Error('發生錯誤');
            }
        }
        let data = await response.json();
        if (data.message === "票券核銷成功。") {
            verifiedTicketCounts=data.data.length;
            console.log(data.data);
            console.log(`${data.message}:${JSON.stringify(data.data)}`);
            showSuccessAlert(data);
        }

    }
    catch (error){
        console.log('核銷過程中發生錯誤',error.message);
    }
}
                