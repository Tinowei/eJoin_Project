/*
const { createApp } = Vue;

const App = {
    components: {
        EasyDataTable: window["vue3-easy-data-table"],
    },
    data() {
        return {
            headers: [
                { text: "活動名稱", value: "Name" },
                { text: "主辦人", value: "Position" },
                { text: "銷售量", value: "Office" },
                { text: "銷售金額", value: "Age", sortable: true },
               { text: "Startdate", value: "Startdate" },
                { text: "銷售金額", value: "Salary" },
            ],
            items: [],
            loading: false,
        };
    },
    methods: {
        getAll() {
            this.loading = true;
            axios.get('https://raw.githubusercontent.com/flyingtrista/FileStorage/main/datatableData.json')
                .then(res => {
                    if (res.status === 200) {
                        toastr.success("Data loaded successfully!");
                        this.items = res.data.data;
                    }
                })
                .catch(error => {
                    console.error(error);
                })
                .finally(() => {
                    this.loading = false;
                });
        },
    },
    mounted() {
        this.getAll();
    },
};

createApp(App).mount("#app");
*/