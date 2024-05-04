const api={getEvents : 'api/Event',updateEvent:'api/Event',getEventForJson:'api/Event/DownloadEventsToJsonFile'}
const apiCaller = {getEvents:()=>httpGet(api.getEvents),updateEvent:()=>httpPut(api.updateEvent),getEventForJson: ()=>httpGet(api.getEventForJson)};

const App = Vue.createApp({
    components: {
        EasyDataTable: window['vue3-easy-data-table'],
    },
    data() {
        return {
            headers: [
                { text: "活動編號", value: "eventId", sortable: true },
                { text: "活動名稱", value: "title" },
                { text: "活動開始時間", value: "startTime", sortable: true },
                { text: "活動結束時間", value: "endTime", sortable: true },
                { text: "舉辦城市", value: "city" },
                { text: "狀態", value: "status" ,sortable: true},
                { text: "主辦人編號", value: "memberId", sortable: true },
                { text: "建立時間", value: "createTime", sortable: true },
                { text: "最後編輯時間", value: "lastEditTime" },
                { text: "", value: "operation" },

            ],
            items:
                [
                    { EventId: 1, Title: "Event 1", StartTime: "2024-04-01 10:00:00", EndTime: "2024-04-01 12:00:00", City: "Taipei", Status: "Active", MemberId: 1, CreateTime: "2024-04-01 08:00:00", LastEditTime: "2024-04-01 08:30:00" },
                    { EventId: 2, Title: "Event 2", StartTime: "2024-04-02 10:00:00", EndTime: "2024-04-02 12:00:00", City: "New York", Status: "Active", MemberId: 2, CreateTime: "2024-04-02 08:00:00", LastEditTime: "2024-04-02 08:30:00" },
                    { EventId: 3, Title: "Event 3", StartTime: "2024-04-03 10:00:00", EndTime: "2024-04-03 12:00:00", City: "London", Status: "Active", MemberId: 3, CreateTime: "2024-04-03 08:00:00", LastEditTime: "2024-04-03 08:30:00" },
                    { EventId: 4, Title: "Event 4", StartTime: "2024-04-04 10:00:00", EndTime: "2024-04-04 12:00:00", City: "Paris", Status: "Active", MemberId: 4, CreateTime: "2024-04-04 08:00:00", LastEditTime: "2024-04-04 08:30:00" },
                    { EventId: 5, Title: "Event 5", StartTime: "2024-04-05 10:00:00", EndTime: "2024-04-05 12:00:00", City: "Tokyo", Status: "Active", MemberId: 5, CreateTime: "2024-04-05 08:00:00", LastEditTime: "2024-04-05 08:30:00" },
                    { EventId: 6, Title: "Event 6", StartTime: "2024-04-06 10:00:00", EndTime: "2024-04-06 12:00:00", City: "Berlin", Status: "Active", MemberId: 6, CreateTime: "2024-04-06 08:00:00", LastEditTime: "2024-04-06 08:30:00" },
                    { EventId: 7, Title: "Event 7", StartTime: "2024-04-07 10:00:00", EndTime: "2024-04-07 12:00:00", City: "Sydney", Status: "Active", MemberId: 7, CreateTime: "2024-04-07 08:00:00", LastEditTime: "2024-04-07 08:30:00" },
                    { EventId: 8, Title: "Event 8", StartTime: "2024-04-08 10:00:00", EndTime: "2024-04-08 12:00:00", City: "Seoul", Status: "Active", MemberId: 8, CreateTime: "2024-04-08 08:00:00", LastEditTime: "2024-04-08 08:30:00" },
                    { EventId: 9, Title: "Event 9", StartTime: "2024-04-09 10:00:00", EndTime: "2024-04-09 12:00:00", City: "Moscow", Status: "Active", MemberId: 9, CreateTime: "2024-04-09 08:00:00", LastEditTime: "2024-04-09 08:30:00" },
                    { EventId: 10, Title: "Event 10", StartTime: "2024-04-10 10:00:00", EndTime: "2024-04-10 12:00:00", City: "Beijing", Status: "Active", MemberId: 10, CreateTime: "2024-04-10 08:00:00", LastEditTime: "2024-04-10 08:30:00" },
                    { EventId: 11, Title: "Event 11", StartTime: "2024-04-11 10:00:00", EndTime: "2024-04-11 12:00:00", City: "Madrid", Status: "Active", MemberId: 11, CreateTime: "2024-04-11 08:00:00", LastEditTime: "2024-04-11 08:30:00" },
                    { EventId: 12, Title: "Event 12", StartTime: "2024-04-12 10:00:00", EndTime: "2024-04-12 12:00:00", City: "Rome", Status: "Active", MemberId: 12, CreateTime: "2024-04-12 08:00:00", LastEditTime: "2024-04-12 08:30:00" },
                    { EventId: 13, Title: "Event 13", StartTime: "2024-04-13 10:00:00", EndTime: "2024-04-13 12:00:00", City: "Cairo", Status: "Active", MemberId: 13, CreateTime: "2024-04-13 08:00:00", LastEditTime: "2024-04-13 08:30:00" },
                    { EventId: 14, Title: "Event 14", StartTime: "2024-04-14 10:00:00", EndTime: "2024-04-14 12:00:00", City: "Athens", Status: "Active", MemberId: 14, CreateTime: "2024-04-14 08:00:00", LastEditTime: "2024-04-14 08:30:00" },
                    { EventId: 15, Title: "Event 15", StartTime: "2024-04-15 10:00:00", EndTime: "2024-04-15 12:00:00", City: "Dubai", Status: "Active", MemberId: 15, CreateTime: "2024-04-15 08:00:00", LastEditTime: "2024-04-15 08:30:00" },
                    { EventId: 16, Title: "Event 16", StartTime: "2024-04-16 10:00:00", EndTime: "2024-04-16 12:00:00", City: "Bangkok", Status: "Active", MemberId: 16, CreateTime: "2024-04-16 08:00:00", LastEditTime: "2024-04-16 08:30:00" },
                    { EventId: 17, Title: "Event 17", StartTime: "2024-04-17 10:00:00", EndTime: "2024-04-17 12:00:00", City: "Singapore", Status: "Active", MemberId: 17, CreateTime: "2024-04-17 08:00:00", LastEditTime: "2024-04-17 08:30:00" },
                    { EventId: 18, Title: "Event 18", StartTime: "2024-04-18 10:00:00", EndTime: "2024-04-18 12:00:00", City: "Istanbul", Status: "Active", MemberId: 18, CreateTime: "2024-04-18 08:00:00", LastEditTime: "2024-04-18 08:30:00" },
                    { EventId: 19, Title: "Event 19", StartTime: "2024-04-19 10:00:00", EndTime: "2024-04-19 12:00:00", City: "Berlin", Status: "Active", MemberId: 19, CreateTime: "2024-04-19 08:00:00", LastEditTime: "2024-04-19 08:30:00" },
                    { EventId: 20, Title: "Event 20", StartTime: "2024-04-20 10:00:00", EndTime: "2024-04-20 12:00:00", City: "Paris", Status: "Active", MemberId: 20, CreateTime: "2024-04-20 08:00:00", LastEditTime: "2024-04-20 08:30:00" },
                    { EventId: 21, Title: "Event 21", StartTime: "2024-04-21 10:00:00", EndTime: "2024-04-21 12:00:00", City: "London", Status: "Active", MemberId: 21, CreateTime: "2024-04-21 08:00:00", LastEditTime: "2024-04-21 08:30:00" },
                    { EventId: 22, Title: "Event 22", StartTime: "2024-04-22 10:00:00", EndTime: "2024-04-22 12:00:00", City: "Tokyo", Status: "Active", MemberId: 22, CreateTime: "2024-04-22 08:00:00", LastEditTime: "2024-04-22 08:30:00" },
                    { EventId: 23, Title: "Event 23", StartTime: "2024-04-23 10:00:00", EndTime: "2024-04-23 12:00:00", City: "New York", Status: "Active", MemberId: 23, CreateTime: "2024-04-23 08:00:00", LastEditTime: "2024-04-23 08:30:00" },
                    { EventId: 24, Title: "Event 24", StartTime: "2024-04-24 10:00:00", EndTime: "2024-04-24 12:00:00", City: "Los Angeles", Status: "Active", MemberId: 24, CreateTime: "2024-04-24 08:00:00", LastEditTime: "2024-04-24 08:30:00" },
                    { EventId: 25, Title: "Event 25", StartTime: "2024-04-25 10:00:00", EndTime: "2024-04-25 12:00:00", City: "Hong Kong", Status: "Active", MemberId: 25, CreateTime: "2024-04-25 08:00:00", LastEditTime: "2024-04-25 08:30:00" },
                    { EventId: 26, Title: "Event 26", StartTime: "2024-04-26 10:00:00", EndTime: "2024-04-26 12:00:00", City: "Shanghai", Status: "Active", MemberId: 26, CreateTime: "2024-04-26 08:00:00", LastEditTime: "2024-04-26 08:30:00" },
                    { EventId: 27, Title: "Event 27", StartTime: "2024-04-27 10:00:00", EndTime: "2024-04-27 12:00:00", City: "Singapore", Status: "Active", MemberId: 27, CreateTime: "2024-04-27 08:00:00", LastEditTime: "2024-04-27 08:30:00" },
                    { EventId: 28, Title: "Event 28", StartTime: "2024-04-28 10:00:00", EndTime: "2024-04-28 12:00:00", City: "Seoul", Status: "Active", MemberId: 28, CreateTime: "2024-04-28 08:00:00", LastEditTime: "2024-04-28 08:30:00" },
                    { EventId: 29, Title: "Event 29", StartTime: "2024-04-29 10:00:00", EndTime: "2024-04-29 12:00:00", City: "Bangkok", Status: "Active", MemberId: 29, CreateTime: "2024-04-29 08:00:00", LastEditTime: "2024-04-29 08:30:00" },
                    { EventId: 30, Title: "Event 30", StartTime: "2024-04-30 10:00:00", EndTime: "2024-04-30 12:00:00", City: "Beijing", Status: "Active", MemberId: 30, CreateTime: "2024-04 - 29 08:00:00", LastEditTime: "2024-04 - 29 08: 30:00" },
                ],
            loading:true,
            eventsRows:[],
            filterText:"",
            editEvent:{},
            eventEdit:null,
            editEventErrorMsg:"",
            editEventValidateState:false,
        }
    },
    methods:{
        //打api (Get)
        getEventList(){
            this.loading=true;
            apiCaller.getEvents()
                .then(response=>{
                    console.log("API Response:", response);
                    this.eventsRows = response.map(e=>{
                        return {
                            eventId:e.id,
                            title:e.title,
                            startTime:this.dateToLocaleString(e.startTime),
                            endTime:this.dateToLocaleString(e.endTime),
                            city:e.city,
                            status:e.status,
                            memberId:e.memberId,
                            createTime:this.dateToLocaleString(e.createTime),
                            lastEditTime:e.lastEditTime ? this.dateToLocaleString(e.lastEditTime) : "未編輯"
                        };
                    });
                    
                    console.log(this.eventsRows);
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
        //更新活動的狀態或是活動名稱
        updateEventAsync() {
            console.log('updateEventAsync is called');
            const editEvent = this.editEvent;
            this.loading = true;
            httpPut(`api/Event/${editEvent.eventId}`, {
                Status: editEvent.status,
                Title: editEvent.title,
                Id: editEvent.eventId,
            })
                .then(() => {
                    this.eventEdit.hide();
                    this.loading = false;
                    console.log(this.editEvent);
                    console.log("更新成功");
                })
                .catch(err => {
                    console.log(editEvent);
                    console.error(err);
                })
                .finally(() => {
                    this.loading = false;
                    this.getEventList();
                });
        },
        downloadEventsToJsonFile(){
            this.loading=true;
            apiCaller.getEventForJson()
                .then(response => {
                    console.log("JOSN檔需要的資料API的Response:", response.result);
                    const jsonStr = JSON.stringify(response.result, null, 2);
                    const date = new Date();
                    const hour = date.getHours()+8;
                    date.setHours(hour);
                    const formattedDate = date.toISOString().split('T')[0].replace(/-/g,'_');
                    const blob = new Blob([jsonStr], {type: "application/json"});
                    console.log("blob",blob);
                    const url = URL.createObjectURL(blob);
                    // 創建一個a標籤用於下載
                    const a = document.createElement('a');
                    a.href = url;
                    a.download = `eJoin_events_${formattedDate}.json`;
                    document.body.appendChild(a);
                    a.click(); // 觸發下載
                    document.body.removeChild(a); // 下載後從文檔中移除a標籤
                    URL.revokeObjectURL(url); // 釋放URL
                })
                .catch(error => {
                    console.error("Error downloading the file", error);
                })
                .finally(() => {
                    this.loading = false;
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
        getCurrentTime(){
            let now = new Date(); 

            let year = now.getFullYear(); 
            let month = now.getMonth() + 1; // 獲取當前月份（0-11，所以加1）
            let day = now.getDate(); 

            let hours = now.getHours();
            let minutes = now.getMinutes();
            let seconds = now.getSeconds(); 

            // 如果月份、日期、小時、分鐘或秒數小於10，前面添加一個'0'
            month = (month < 10) ? "0" + month : month;
            day = (day < 10) ? "0" + day : day;
            hours = (hours < 10) ? "0" + hours : hours;
            minutes = (minutes < 10) ? "0" + minutes : minutes;
            seconds = (seconds < 10) ? "0" + seconds : seconds;
            
            let dateTime = year + "/" + month + "/" + day + " " + hours + ":" + minutes + ":" + seconds;
            return dateTime;
        },
        validateTitleLength(value) {
            return /^.{1,30}$/.test(value.trim());
        },
        editEventModal(id){
            const event = this.eventsRows.find(x => x.eventId === id);
            if (event) {
                this.editEvent = JSON.parse(JSON.stringify(event));
                console.log(this.editEvent);
            } else {
                console.error('No event found with the given ID.');
            }
        },
        openEditModal(event) {
            this.editEvent = event;
        },
    },
    mounted(){
        console.log(this.getCurrentTime());
        this.getEventList();
        this.eventEdit = new bootstrap.Modal(this.$refs.eventEditModal, {
            keyboard: false
        });
        // this.$nextTick(() => {
        //     try {
        //         this.eventEdit = new bootstrap.Modal(this.$refs.eventEditModal, {
        //             keyboard: false
        //         });
        //     } catch (error) {
        //         console.error('mounted 鉤子錯誤:', error);
        //     }
        // });
    },
    computed: {
        filteredEvents() {
            console.log("Filter Text:", this.filterText);
            let filter = new RegExp(this.filterText, 'i')
            return this.eventsRows.filter(event => {
                return Object.keys(event).some(key =>{
                    if(this.headers.some(header=>header.value === key && header.sortable)){
                        let value = event[key];
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
    watch:{
        'editEvent.title': {
            handler(val) {
                this.editEventValidateState = this.validateTitleLength(val);
                if (this.editEventValidateState) {
                    this.editEventErrorMsg = "";
                } else {
                    this.editEventErrorMsg = "Description length should be 1-30 characters.";
                }
            },
            immediate: false
        }
    }
}).mount('#app');