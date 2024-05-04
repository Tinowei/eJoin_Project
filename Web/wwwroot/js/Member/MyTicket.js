
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

    //一網頁有多分頁時，防止回頂端改變網址，造成被彈回預設頁面
    document.addEventListener('DOMContentLoaded', function() {
        const top = document.querySelector('.top');

        top.addEventListener('click', (event) => {
            event.preventDefault(); 
            scrollToTop();
        });

        function scrollToTop() {
            const scroll = document.documentElement.scrollTop || document.body.scrollTop;
            if (scroll > 0) {
                window.scrollTo({
                    top: 0,
                    behavior: 'smooth' /*平滑捲動*/
                });
            }
        }
    });



// 存儲選擇的票劵數量，儲存到localStorage(對應getTicket頁面)
    document.getElementById('confirmQuantityBtn').addEventListener('click', function() {
    const quantity = document.getElementById('quantityInput').value;
    localStorage.setItem('selectedQuantity', quantity);
  });

    
    
    //測試
//將數量打回後端api
//     var selectedNumber={ number : 0 };
//     const confirmQunatityBtn = document.getElementById('confirmQuantityBtn');
//    
//     confirmQunatityBtn.addEventListener('click',function(){
//         let quantity = document.getElementById('quantityInput').value;
//         selectedNumber.number=quantity;
//         if (isNaN(quantity) || quantity < 1) {
//             console.error('沒有選擇數量');
//             return;
//         }
//         console.log(selectedNumber.number);
//         //CallUseTicketApi();
//        
//     })
    
    
    
// async function CallUseTicketApi(){
//     let requestBody = JSON.stringify(selectedNumber);
//     try {
//         let options={
//             method:"POST",
//             body:requestBody,
//             headers: { 'Content-Type': 'application/json' } 
//         }
//         let response = await fetch("/api/Ticket/UseTicket",options);
//         if (!response.ok) {
//             throw new Error(`HTTP error! status: ${response.status}`);
//         }
//         let data=await response.json();
//         let imageDataUrl = data.ImageDataUrl;
//        
//         // console.log('傳回來的圖片是：',data.imageDataUrl);
//         // alert(data.imageDataUrl);
//        
//         // //store in localStorage
//         // localStorage.setItem('QrCodeImg',data.imageDataUrl);
//         // // window.location.assign(`/Ticket/?imageData=${data.imageDataUrl}`);
//         // window.location.assign(`/Ticket/?number=${localStorage.getItem('selectedQuantity')}`);
//
//         if (data.success===true) {
//             console.log('傳回來的圖片是：', data.imageDataUrl);
//             alert(data.imageDataUrl);
//             localStorage.setItem('QrCodeImg', data.imageDataUrl);
//             window.location.assign(`/Ticket/?number=${localStorage.getItem('selectedQuantity')}`);
//         } else {
//             console.log(data);
//             alert(data.errorMessage);
//             // 不跳轉
//         }
//        
//     }
//     catch (err){
//         console.log(err);
//     }
// }   



//test CallGetMemberTicketsApi
    let releaseTickets=[];
    let usedTickets=[];
    let orderList=[];
window.onload=async function(){
        // await CallGetMemberTicketsApi();
        createVueApp();
}    


async function CallGetMemberTicketsApi(){
        try{
            let options={
                method: "GET",
                headers:{'Content-Type':'application/json'}
            };
            let response = await fetch('/api/Ticket/GetMemberTickets',options);
            if (!response.ok) {
                throw new Error(`HTTP error! status: ${response.status}`);
            }
            let data=await response.json();
            releaseTickets=data;
            console.log(data);
        }catch (err){
            console.error(err);
        }
}
function createVueApp(){
    const { createApp } = Vue;

    const app = createApp({
        data() {
            return {
                selectedNumber: { number: 1 },
                releaseTickets:releaseTickets,
                selectedTicket:null,
                usedTickets:usedTickets,
                orderList:{
                    orders:[],
                    currentPage: 1, // 當前頁碼
                    totalPage: 1, // 總頁數
                },
                api:{usedTickets:'/api/Ticket/GetMemberUsedTickets',
                    getMemberTicketsCount:'/api/Ticket/GetMemberTicketsCount',
                    callGetMemberTicketsApi:'/api/Ticket/GetMemberTickets',
                    callGetUsedTicketsCountApi:'/api/Ticket/GetUsedTicketsCount',
                    callUseTicketApi:'/api/Ticket/UseTicket',
                    callGetMemberOrdersApi:'/api/Ticket/GetMemberOrdersByPagedList',
                },
                pageList:{
                    releaseTickets: {
                        startPage: 1,
                        endPage: 0,
                        currentPage: 1,
                        pageSize: 3,
                        totalPages: 0,
                        totalCount: 0
                    },
                    usedRecords: {
                        startPage: 1,
                        endPage: 0,
                        currentPage: 1,
                        pageSize: 3,
                        totalPages: 0,
                        totalCount: 0
                    },
                },
                currentTab: 'releaseTickets',
                ordersLoaded: false,
            }
        },
        mounted() {
            // this.CallGetReleaseTicketsByPagedList(1);
            //todo:做releaseTicket的分頁數
            this.SetCurrentTab('releaseTickets');
            this.CallGetMemberTicketsCountApi();
            this.CallGetMemberTicketsApi();
            this.CallGetMemberOrdersApi();
        },
        created(){
            // this.CallGetMemberOrdersApi();
        },
        watch:{
          'selectedNumber.number':function(){
              if(this.selectedTicket){
                  this.saveExChangeTicketsInfoToSession(this.selectedTicket);
              }
          }  
        },
        methods: {
            
            selectTicket(ticket){
              this.selectedTicket=ticket;
              // this.selectedNumber.number=1;
              console.log(this.selectedTicket);
              let ticketInfo = this.selectedTicket;
              console.log(ticketInfo);
              this.saveExChangeTicketsInfoToSession(ticketInfo);
            },
            saveExChangeTicketsInfoToSession(ticket){
                let quantities = this.selectedNumber.number;
                const ticketInfo = {
                    eventId:ticket.eventId,
                    eventName: ticket.eventName,
                    quantities: quantities, 
                    expireTime: ticket.expireTime,
                    ticketTypeName: ticket.ticketTypeName,
                    participantName: ticket.participantName,
                    participantEmail: ticket.participantEmail,
                    participantPhone: ticket.participantPhone
                };
                sessionStorage.setItem('exchangeTicketsInfo',JSON.stringify(ticketInfo));
            },
            confirmQuantity() {
                let quantity = this.selectedNumber.number;
                if (isNaN(quantity) || quantity < 1) {
                    console.error('沒有選擇數量');
                    return;
                }
                console.log(this.selectedNumber.number);
                console.log('Selected quantity:', this.selectedNumber.number);
                console.log('Selected ticket:', this.selectedTicket);
                console.log('Selected ticket updated:', this.selectedTicket);
                this.CallUseTicketApi();
            },
            showNotEnoughAlert(){
                Swal.fire({
                    icon: 'warning',
                    title: '【庫存不足無法兌換】',
                    text: '請重新選擇數量',
                    showConfirmButton: true,
                    confirmButtonText:'確認',
                    heightAuto: false,
                    customClass: {
                        title: 'S2_title_class',
                        content: 'S2_content_class',
                    }
                })},
            showSuccessUseTicketAlert(){
                Swal.fire({
                    icon: 'success',
                    title: '【成功兌換】',
                    text: '即將跳轉至Qrocde頁面...',
                    showConfirmButton: false,
                    // confirmButtonText:'確認',
                    timer:2000,
                    heightAuto: false,
                    customClass: {
                        title: 'S2_title_class',
                        content: 'S2_content_class',
                    }
                }).then(()=>{
                    let exChangeTicketsInfo = JSON.parse(sessionStorage.getItem('exchangeTicketsInfo'));
                    let quantities =  exChangeTicketsInfo.quantities;
                    window.location.assign(`/Ticket?quantities=${quantities}`);
                });
            },
            async CallGetMemberTicketsApi() {
                let pagination = this.pageList[this.currentTab];
                try {
                    let options = {
                        method: "GET",
                        headers: { 'Content-Type': 'application/json' }
                    };
                    let response = await fetch(`${this.api.callGetMemberTicketsApi}?page=${pagination.currentPage}&pageSize=${pagination.pageSize}`, options);
                    if (!response.ok) {
                        throw new Error(`HTTP error! status: ${response.status}`);
                    }
                    this.releaseTickets = await response.json();
                    console.log("callGetMemberTicketsApi的結果：",this.releaseTickets);
                    
                    let pageUrl=`${window.location.protocol}//${window.location.host}${window.location.pathname}#registered?page=${pagination.currentPage}&pageSize=${pagination.pageSize}`;
                    window.history.pushState({path:pageUrl},'',pageUrl);
                } catch (err) {
                    console.error(err);
                }
            },
            async CallGetMemberTicketsCountApi(){
                let pagination = this.pageList[this.currentTab];
                try{
                    let options={
                        method:"GET",
                        header: {'Content-Type':'application/json'}
                    };
                    let response = await fetch(`${this.api.getMemberTicketsCount}`,options);
                    if(!response.ok){
                        throw new Error(`HTTP error! status: ${response.status}`);                    
                    }
                    let totalCount = await response.json();
                    // console.log("已發行未使用之票券的數量",totalCount);
                    pagination.totalCount=totalCount;
                    pagination.totalPages=Math.ceil(pagination.totalCount/pagination.pageSize);
                    console.log('已發行未使用票券數:', pagination.totalCount);
                    console.log('總頁數：',pagination.totalPages);
                }catch(err){
                    console.error(err);
                }
            },
            async CallUseTicketApi(){
                let requestBody = JSON.stringify({SelectedNumber: this.selectedNumber.number,
                    EventId: this.selectedTicket.eventId,
                    TicketTypeId: this.selectedTicket.ticketTypeId});
                
                try {
                    let options={
                        method:"POST",
                        body:requestBody,
                        headers: { 'Content-Type': 'application/json' }
                    }
                    let response = await fetch(`${this.api.callUseTicketApi}`,options);
                    if (!response.ok) {
                        throw new Error(`HTTP error! status: ${response.status}`);
                    }
                    let data=await response.json();
                    let imageDataUrl = data.ImageDataUrl;

                    if (data.success===true) {
                        console.log('傳回來的圖片是：', data.imageDataUrl);
                        // alert(data.imageDataUrl);
                        localStorage.setItem('QrCodeImg', data.imageDataUrl);
                        this.showSuccessUseTicketAlert();
                    } else {
                        console.log(data);
                        console.log(data.errorMessage);
                        this.showNotEnoughAlert();
                        // 不跳轉
                    }

                }
                catch (err){
                    console.log(err);
                }
            },
            //請求已使用過的票券
            async CallUsedTicketsApi(){
                let pagination = this.pageList[this.currentTab];
                try{
                    let options={method:"GET",header:{'Content-Type':'application/json'}};
                    let response=await fetch(`${this.api.usedTickets}?page=${pagination.currentPage}&pageSize=${pagination.pageSize}`,options);
                    if(!response.ok){
                        throw new Error(`Http error !  status: ${response.status}`);
                    }
                    let data = await response.json();
                    this.usedTickets = data.result.value;
                    console.log(this.usedTickets);

                    let pageUrl=`${window.location.protocol}//${window.location.host}${window.location.pathname}#useTicket?page=${pagination.currentPage}&pageSize=${pagination.pageSize}`;
                    window.history.pushState({path:pageUrl},'',pageUrl);
                }
                catch (err){
                    console.log(err);
                }
            },
            async CallGetUsedTicketsCountApi() {
                let pagination = this.pageList[this.currentTab];
                try {
                    let options = {
                        method: "GET",
                        headers: { 'Content-Type': 'application/json' }
                    };
                    let response = await fetch(`${this.api.callGetUsedTicketsCountApi}`, options);
                    if (!response.ok) {
                        throw new Error(`HTTP error! status: ${response.status}`);
                    }
                    let totalCount = await response.json();
                    console.log(totalCount);
                    pagination.totalCount=totalCount;
                    pagination.totalPages = Math.ceil(pagination.totalCount / pagination.pageSize);
                    console.log('已使用票券卡片筆數:', pagination.totalCount);
                    console.log('總頁數:', pagination.totalPages);
                    
                } catch (err) {
                    console.error(err);
                }
            },
            //請求購買記錄
            async CallGetMemberOrdersApi(page=1){
                try{
                    let options={method:"GET",header:{'Content-Type':'application/json'}};
                    let response = await fetch(`${this.api.callGetMemberOrdersApi}?page=${page}`,options);
                    if(!response.ok){
                        throw new Error(`api發生錯誤! status: ${response.status}`);
                    }
                    let data = await response.json();
                    this.orderList.orders = [...data.orders];;
                    this.orderList.totalPage=data.totalPage;
                    this.orderList.currentPage=page;
                    console.log('callGetMemberOrdersApi的結果:',this.orderList);
                    console.log('總頁數',this.orderList.totalPage);
                    console.log('目前頁數',this.orderList.currentPage);

                    let pageUrl=`${window.location.protocol}//${window.location.host}${window.location.pathname}#buyTicket?page=${this.orderList.currentPage}`;
                    window.history.pushState({path:pageUrl},'',pageUrl);
                }
                catch (err){
                    console.error(err);
                }
            },
            
            async ChangePage(page) {
                let pagination = this.pageList[this.currentTab];
                const maxPageToShow = 3;
                if (page < 1 || page > pagination.totalPages) {
                    return; 
                }
                pagination.currentPage = page;
                //
                let startPage , endPage;
                if(pagination.totalPages <=maxPageToShow){
                    startPage =1;
                    endPage=pagination.totalPages;
                }
                else{
                    startPage = Math.max(page-1,1);
                    endPage = Math.min(page+1,pagination.totalPages);

                    if(page===1){
                        endPage = startPage + maxPageToShow - 1;
                    }
                    else if(page === pagination.totalPages){
                        startPage = endPage - maxPageToShow + 1;
                    }
                }
                pagination.startPage = startPage;
                pagination.endPage=endPage;

                if (this.currentTab === 'usedRecords') {
                    await this.CallUsedTicketsApi();
                } else if (this.currentTab === 'releaseTickets') {
                    await this.CallGetMemberTicketsApi();
                }else if(this.currentTab ==='orderDetails'){
                    await this.CallGetMemberOrdersApi();
                }
            },
            async ChangePageForOrderDetail(page) {
                if (page < 1 || page > this.orderList.totalPage) {
                    return; // 如果页码无效，则不执行任何操作
                }
                this.orderList.currentPage = page;
                // 调用相应的API来获取新的订单数据
                await this.CallGetMemberOrdersApi(page);
            },
            SetCurrentTab(tabName) {
                this.currentTab = tabName;
                
            },
            //測試
            async CallGetReleaseTicketsByPagedList(page){
              try{
                  let options = {
                      method: "GET",
                      headers: { 'Content-Type': 'application/json' }
                  };
                  let response = await fetch(`/api/Ticket/GetReleaseTicketsByPageList?page=${page}`, options);
                  if (!response.ok) {
                      throw new Error(`HTTP error! status: ${response.status}`);
                  }
                  let releaseTickets = await response.json();
                  console.log("GetReleaseTicketsByPageList的結果：", releaseTickets);
              }catch (err) {
                  console.error(err);
              }
            },
        },
        computed: {
            PagesToShow() {
                let pagination = this.pageList[this.currentTab];
                let pages = [];
                let startPage = Math.max(pagination.currentPage - 1, 1);
                let endPage = Math.min(pagination.currentPage + 1, pagination.totalPages);

                if (pagination.totalPages <= 3) {
                    // 如果總頁數不超過3，顯示所有頁碼
                    for (let i = 1; i <= pagination.totalPages; i++) {
                        pages.push(i);
                    }
                } else {
                    // 確保總是顯示3個頁碼
                    if (pagination.currentPage === 1) {
                        // 如果是第一頁，顯示1到3
                        for (let i = 1; i <= 3; i++) {
                            pages.push(i);
                        }
                    } else if (pagination.currentPage === pagination.totalPages) {
                        // 如果是最後一頁，顯示最後三個頁碼
                        for (let i = pagination.totalPages - 2; i <= pagination.totalPages; i++) {
                            pages.push(i);
                        }
                    } else {
                        // 否則，顯示當前頁碼的前後各一頁
                        pages.push(startPage);
                        pages.push(pagination.currentPage);
                        pages.push(endPage);
                    }
                }
                return pages;
            },
            PagesToShowForOrderDetail() {
                let pages = [];
                let startPage, endPage;

                if (this.orderList.totalPage <= 3) {
                    // 如果總頁數小於3，顯示所有頁碼
                    startPage = 1;
                    endPage = this.orderList.totalPage;
                } else {
                    //若總頁數>3，計算開始和結束頁碼
                    if (this.orderList.currentPage <= 2) {
                        startPage = 1;
                        endPage = 3;
                    } else if (this.orderList.currentPage >= this.orderList.totalPage - 1) {
                        startPage = this.orderList.totalPage - 2;
                        endPage = this.orderList.totalPage;
                    } else {
                        startPage = this.orderList.currentPage - 1;
                        endPage = this.orderList.currentPage + 1;
                    }
                }

                // 產生頁碼[]
                for (let i = startPage; i <= endPage; i++) {
                    pages.push(i);
                }

                return pages;
            }
        }
    });

    app.mount('#app');
}




