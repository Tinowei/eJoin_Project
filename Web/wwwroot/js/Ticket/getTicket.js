
    //當已經在頂端，回頂端符號隱藏
    const arrow = document.querySelector('.top');  /*透過選擇器(.top)，取得DOM元素，放進變數arrow*/
        window.addEventListener('scroll', () =>     /*針對「視窗」，新增「滾動」這個事件，進行監聽*/
            {
                const scroll = window.scrollY;         /*將滾動視窗y軸這件事，放進變數scroll*/

                if (scroll > 0) {                           /*如果變數scroll(滾動視窗y軸)大於0*/
                    arrow.classList.add('visible');         /*就在變數arrow的class，新增一個叫做visible的class*/   
                } else {                                    /*否則移除叫做visible的class*/
                    arrow.classList.remove('visible');
                }
            }
);

// // 讀取從MyTicket選擇數量，並存到localStorage的資料
// const selectedQuantity = localStorage.getItem('selectedQuantity');
// if (selectedQuantity) {
//     document.querySelector('.useNember').innerText = ' 使用張數： ' + selectedQuantity;
// }

//抓出local的QrCodeImg Value

const qrCodeImg = document.querySelector('.qrCodeImg');
let qrCode = localStorage.getItem('QrCodeImg');
if(qrCode){
    qrCodeImg.src=qrCode
}






const { createApp } = Vue;
const app=createApp({
    data(){
        return{
            ticketInfo:{}
        };
    },
    mounted(){
        let exChangeTicketsInfo = JSON.parse(sessionStorage.getItem('exchangeTicketsInfo'));
        console.log(exChangeTicketsInfo);
        this.ticketInfo=exChangeTicketsInfo;
        console.log('ticketInfo:',this.ticketInfo)
    }
});
app.mount('#app');
    