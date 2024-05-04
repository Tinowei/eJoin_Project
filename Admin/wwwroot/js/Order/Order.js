const api={getOrders : 'api/Order/GetOrders'}
const apiCaller = {getOrders:()=>httpGet(api.getOrders)};


const App = Vue.createApp({
    components: {
        EasyDataTable: window['vue3-easy-data-table'],
    },
    data() {
        return {
            createData: {
                OrderId: '',
                EventTitle: '',
                ParticipantName: '',
                ParticipantEmail: '',
                ParticipantPhone: '',
                TotalMoney: '',
                CreateTime: 0,
                DetailsButton: '',
            },
            currentData: {
                OrderId: '',
                EventTitle: '',
                ParticipantName: '',
                ParticipantEmail: '',
                ParticipantPhone: '',
                TotalMoney: '',
                CreateTime: 0,
                DetailsButton: '',
            },
            headers: [
                { text: "訂單編號", value: "orderNo", sortable: true },
                { text: "活動名稱", value: "eventName" },
                { text: "參加者姓名", value: "participantName"},
                { text: "參加者電子郵件", value: "participantEmail"},
                { text: "參加者手機", value: "participantPhone" },
                { text: "總金額", value: "totalMoney" },
                { text: "購入時間", value: "createTime", sortable: true },
                { text: "明細", value: "detail",  },
            ],
            items:
                [
                    {
                        OrderId: "1001",
                        EventTitle: "活動1",
                        ParticipantName: "參加者1",
                        ParticipantEmail: "participant1@example.com",
                        ParticipantPhone: "123456789",
                        TotalMoney: 1000,
                        CreateTime: "2024-04-11",

                    },
                    {
                        OrderId: "1002",
                        EventTitle: "活動2",
                        ParticipantName: "參加者2",
                        ParticipantEmail: "participant2@example.com",
                        ParticipantPhone: "987654321",
                        TotalMoney: 2000,
                        CreateTime: "2024-04-12",

                    },
                    {
                        OrderId: "1003",
                        EventTitle: "活動3",
                        ParticipantName: "參加者3",
                        ParticipantEmail: "participant3@example.com",
                        ParticipantPhone: "135792468",
                        TotalMoney: 3000,
                        CreateTime: "2024-04-13",

                    }
                ],
            loading:true,
            ordersRows:[],
            filterText:"",
            showModal: false,
            selectedOrder: {},
        }
    },
    methods:{
        //打api (Get)
        getOrderList(){
            this.loading=true;
            apiCaller.getOrders()
                .then(response=>{
                    console.log("API Response:", response);
                    this.ordersRows = response.map(o=>{
                        return {
                            // orderId:o.orderId,
                            orderNo: o.orderNo,
                            participantName: o.participantName,
                            participantPhone: o.participantPhone,
                            participantEmail: o.participantEmail, 
                            // orderDetailId: o.orderDetailId,
                            eventName: o.eventName,
                            createTime: this.dateToLocaleString(o.createTime),
                            totalMoney: o.totalMoney,
                            tickets: o.tickets.map(ticket => {
                                return {
                                    ticketTypeId: ticket.ticketTypeId,
                                    ticketTypeName: ticket.ticketTypeName,
                                    orderDetailId: ticket.orderDetailId,
                                    unitPrice: ticket.unitPrice,
                                    purchaseQuantity: ticket.purchaseQuantity
                                };
                            })
                        };
                    });
                    console.log(this.ordersRows);
                })
                .catch(err=>{
                    console.error(err);
                })
                .finally(()=>{
                    this.loading=false;
                    // this.editEvent=this.eventsRows
                    console.log("成功進入Finally")
                });
        },
        dateToLocaleString(date) {
            if (!date) {
                return "null";
            }
            const dateObj = new Date(date);
            return dateObj.toLocaleString([],{
                year:'numeric',
                month:'2-digit',
                day: '2-digit',
                hour:'2-digit',
                minute:'2-digit',
            });
        },
        showOrderDetails(item) {
            this.selectedOrder = item;
            console.log(this.selectedOrder);
            console.log(this.selectedOrder.tickets);
        },
    },
    mounted(){
        this.getOrderList();
    },
    computed: {
        filteredEvents() {
            console.log("Filter Text:", this.filterText);
            let filter = new RegExp(this.filterText, 'i')
            return this.ordersRows.filter(o => {
                return Object.keys(o).some(key =>{
                    if(this.headers.some(header=>header.value === key && header.sortable)){
                        let value = o[key];
                        if(value != null && value !==undefined){
                            value = value.toString();
                        }
                        else{
                            return false;
                        }
                        return value.match(filter);
                    }
                    return false;
                });
            });
        }
    },

});

// App.component('order-detail-modal', {
//     template: '#order-detail-modal-template',
//     props: {
//         order: Object,
//         showModal: Boolean
//     }
// });

App.mount('#app');