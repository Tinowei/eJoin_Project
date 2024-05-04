
const App = Vue.createApp({
    components: {
        EasyDataTable: window['vue3-easy-data-table'],
    },
    data() {
        return {
            searchKeyword: '',
            headers: [
                { text: "會員編號", value: "MemberId", sortable: true },
                { text: "電子郵件(帳號)", value: "Email" },
                { text: "真實姓名", value: "Name" },
                { text: "顯示名稱", value: "DisplayName" },
                { text: "手機", value: "Phone" },
                { text: "註冊時間", value: "RegisterTime", sortable: true },
                { text: "修改時間", value: "LastEditTime", sortable: true },
            ],
            items: [],
         
             //items:
             //[
        //         { MemberId: 1, Email: "example1@example.com", Name: "貓貓", DisplayName: "John", Phone: "123-456-7890", RegisterTime: "2024-04-01", LastEditTime: "2024-04-05" },
        //         { MemberId: 2, Email: "example2@example.com", Name: "Jane Doe", DisplayName: "Jane", Phone: "234-567-8901", RegisterTime: "2024-04-02", LastEditTime: "2024-04-06" },
        //         { MemberId: 3, Email: "example3@example.com", Name: "Alice Smith", DisplayName: "Alice", Phone: "345-678-9012", RegisterTime: "2024-04-03", LastEditTime: "2024-04-07" },
        //         { MemberId: 4, Email: "example4@example.com", Name: "Bob Johnson", DisplayName: "Bob", Phone: "456-789-0123", RegisterTime: "2024-04-04", LastEditTime: "2024-04-08" },
        //         { MemberId: 5, Email: "example5@example.com", Name: "Emily Brown", DisplayName: "Emily", Phone: "567-890-1234", RegisterTime: "2024-04-05", LastEditTime: "2024-04-09" },
        //         { MemberId: 6, Email: "example6@example.com", Name: "James Wilson", DisplayName: "James", Phone: "678-901-2345", RegisterTime: "2024-04-06", LastEditTime: "2024-04-10" },
        //         { MemberId: 7, Email: "example7@example.com", Name: "Emma Jones", DisplayName: "Emma", Phone: "789-012-3456", RegisterTime: "2024-04-07", LastEditTime: "2024-04-11" },
        //         { MemberId: 8, Email: "example8@example.com", Name: "William Davis", DisplayName: "William", Phone: "890-123-4567", RegisterTime: "2024-04-08", LastEditTime: "2024-04-12" },
        //         { MemberId: 9, Email: "example9@example.com", Name: "Olivia Taylor", DisplayName: "Olivia", Phone: "901-234-5678", RegisterTime: "2024-04-09", LastEditTime: "2024-04-13" },
        //         { MemberId: 10, Email: "example10@example.com", Name: "Michael Anderson", DisplayName: "Michael", Phone: "012-345-6789", RegisterTime: "2024-04-10", LastEditTime: "2024-04-14" },
        //         { MemberId: 11, Email: "example11@example.com", Name: "Sophia Martinez", DisplayName: "Sophia", Phone: "123-456-7890", RegisterTime: "2024-04-11", LastEditTime: "2024-04-15" },
        //         { MemberId: 12, Email: "example12@example.com", Name: "Alexander Hernandez", DisplayName: "Alexander", Phone: "234-567-8901", RegisterTime: "2024-04-12", LastEditTime: "2024-04-16" },
        //         { MemberId: 13, Email: "example13@example.com", Name: "Isabella Lopez", DisplayName: "Isabella", Phone: "345-678-9012", RegisterTime: "2024-04-13", LastEditTime: "2024-04-17" },
        //         { MemberId: 14, Email: "example14@example.com", Name: "Matthew Gonzalez", DisplayName: "Matthew", Phone: "456-789-0123", RegisterTime: "2024-04-14", LastEditTime: "2024-04-18" },
        //         { MemberId: 15, Email: "example15@example.com", Name: "Evelyn Perez", DisplayName: "Evelyn", Phone: "567-890-1234", RegisterTime: "2024-04-15", LastEditTime: "2024-04-19" },
        //         { MemberId: 16, Email: "example16@example.com", Name: "Daniel Moore", DisplayName: "Daniel", Phone: "678-901-2345", RegisterTime: "2024-04-16", LastEditTime: "2024-04-20" },
        //         { MemberId: 17, Email: "example17@example.com", Name: "Mia Taylor", DisplayName: "Mia", Phone: "789-012-3456", RegisterTime: "2024-04-17", LastEditTime: "2024-04-21" },
        //         { MemberId: 18, Email: "example18@example.com", Name: "David Anderson", DisplayName: "David", Phone: "890-123-4567", RegisterTime: "2024-04-18", LastEditTime: "2024-04-22" },
        //         { MemberId: 19, Email: "example19@example.com", Name: "Sophia Garcia", DisplayName: "Sophia", Phone: "901-234-5678", RegisterTime: "2024-04-19", LastEditTime: "2024-04-23" },
        //         { MemberId: 20, Email: "example20@example.com", Name: "Joseph Rodriguez", DisplayName: "Joseph", Phone: "012-345-6789", RegisterTime: "2024-04-20", LastEditTime: "2024-04-24" },
        //         { MemberId: 21, Email: "example21@example.com", Name: "Olivia Hernandez", DisplayName: "Olivia", Phone: "123-456-7890", RegisterTime: "2024-04-21", LastEditTime: "2024-04-25" },
        //         { MemberId: 22, Email: "example22@example.com", Name: "William Wilson", DisplayName: "William", Phone: "234-567-8901", RegisterTime: "2024-04-22", LastEditTime: "2024-04-26" },
        //         { MemberId: 23, Email: "example23@example.com", Name: "Daniel Martinez", DisplayName: "Daniel", Phone: "345-678-9012", RegisterTime: "2024-04-23", LastEditTime: "2024-04-27" },
        //         { MemberId: 24, Email: "example24@example.com", Name: "Emily Lopez", DisplayName: "Emily", Phone: "456-789-0123", RegisterTime: "2024-04-24", LastEditTime: "2024-04-28" },
        //         { MemberId: 25, Email: "example25@example.com", Name: "Joseph Brown", DisplayName: "Joseph", Phone: "567-890-1234", RegisterTime: "2024-04-25", LastEditTime: "2024-04-29" },
        //         { MemberId: 26, Email: "example26@example.com", Name: "Mia Gonzalez", DisplayName: "Mia", Phone: "678-901-2345", RegisterTime: "2024-04-26", LastEditTime: "2024-04-30" },
        //         { MemberId: 27, Email: "example27@example.com", Name: "David Perez", DisplayName: "David", Phone: "789-012-3456", RegisterTime: "2024-04-27", LastEditTime: "2024-05-01" },
        //         { MemberId: 28, Email: "example28@example.com", Name: "Evelyn Moore", DisplayName: "Evelyn", Phone: "890-123-4567", RegisterTime: "2024-04-28", LastEditTime: "2024-05-02" },
        //         { MemberId: 29, Email: "example29@example.com", Name: "Michael Taylor", DisplayName: "Michael", Phone: "901-234-5678", RegisterTime: "2024-04-29", LastEditTime: "2024-05-03" },
        //         { MemberId: 30, Email: "example30@example.com", Name: "Olivia Anderson", DisplayName: "Olivia", Phone: "012-345-6789", RegisterTime: "2024-04-30", LastEditTime: "2024-05-04" },
        //     ],

        }
    },
    created() {
        this.getAll(); // 在 Vue 實例建立時自動抓取資料
    },
    methods: {
        //關鍵字搜尋
        search() {
            console.log("search")
            //將關鍵字轉成小寫
            const keyword = this.searchKeyword.toLowerCase();
            //如果沒有輸入關鍵字，就顯示全部資料
            if (!keyword) {
                this.getAll();
                return;
            }
            //當有輸入關鍵字，篩選出符合關鍵字的這些內容
            this.items = this.items.filter(item => {
                return Object.values(item).some(value => {
                    if (typeof value === "string") {
                        return value.toLowerCase().includes(keyword);
                    }
                    return false;
                });
            });
        },
        getAll() {
            axios.get('/api/Member/GetMemberData')
                .then(res => {
                    if (res.status == 200) {
                        this.items = res.data.map(e => {
                            return {
                                MemberId: e.memberId,
                                Email: e.email,
                                Name: e.name,
                                DisplayName: e.displayName,
                                Phone: e.phone,
                                RegisterTime: this.dateToLocaleString(e.registerTime),
                                LastEditTime: this.dateToLocaleString(e.lastEditTime)
                            };
                        });
                    }
                })
                .catch(error => {
                    console.error(error);
                })
        },
        dateToLocaleString(date) {
            //如果日期不存在，顯示null
            if (!date) {
                return "無修改紀錄";
            }
            //將日期轉換成當地時間
            const dateObj = new Date(date);
            return dateObj.toLocaleString([], {
                year: 'numeric', //年份顯示數字
                month: '2-digit', //月份顯示2位數字
                day: '2-digit',
                hour: '2-digit',
                minute: '2-digit',
            });
        },
    }
}).mount('#app');