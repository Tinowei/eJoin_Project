const { createApp } = Vue;

const app = createApp({
    data() {
        return {
            requsetData: {
                selectedOrderBy: "focus",
                selectedPrice: null,
                selectedTime: null,
                selectedThemes: [],
                selectedPlaces: [],
                currentPage: 1,
            },
            events: [
                {
                    eventId: 1,
                    eventCoverUrl: "https://res.cloudinary.com/dlhaqaap3/image/upload/v1712025610/ImageUploadByApi/tu8utesrztfffboqteli.jpg",
                    eventStartDate: "2024-04-09",
                    eventEndDate: "2024-05-01",
                    eventTitle: "Tech Conference1231231231",
                    eventCity: "台北市",
                    eventThemes: [
                        "科技", "公益"
                    ],
                    isLike: true,
                    heartCount: 5,
                }
            ],
        };
    },
    methods: {
        orderBySelectOnChange(e) {
            this.selectedOrderBy = e.currentTarget.value;
            this.display();
        },
        priceBtnOnclick(e) {
            if (e.currentTarget.classList.contains("checked")) {
                e.currentTarget.classList.remove("checked");
                this.requsetData.selectedPrice = null;
                return;
            }
            this.requsetData.selectedPrice = e.currentTarget.dataset.price;
            this.display();
        },
        timeBtnOnclick(e) {
            if (e.currentTarget.classList.contains("checked")) {
                e.currentTarget.classList.remove("checked");
                this.requsetData.selectedTime = null;
                return;
            }
            this.requsetData.selectedTime = e.currentTarget.dataset.time;
            this.display();
        },
        themeBtnOnclick(e) {
            if (e.currentTarget.classList.contains("checked")) {
                e.currentTarget.classList.remove("checked");
                this.requsetData.selectedThemes = this.requsetData.selectedThemes.filter((theme) => theme != e.currentTarget.dataset.themeId);
                this.display();
                return;
            }
            this.requsetData.selectedThemes.push(e.currentTarget.dataset.themeId);
            e.currentTarget.classList.add("checked");
            this.display();
        },
        placeBtnOnclick(e) {
            if (e.currentTarget.classList.contains("checked")) {
                e.currentTarget.classList.remove("checked");
                this.requsetData.selectedPlaces = this.requsetData.selectedPlaces.filter((place) => place != e.currentTarget.dataset.place);
                this.display();
                return;
            }

            this.requsetData.selectedPlaces.push(e.currentTarget.dataset.place);
            e.currentTarget.classList.add("checked");
            this.display();
        },
        display() {
            console.log(this.requsetData);
        },
        async sentFillter() {
            if (this.requsetData.selectedTime == 'customize') this.requsetData.selectedTime = this.$refs.dateRange.value;
            let apiPath = "/api/Search"
            let returnObject = await (this.callApi(apiPath, this.requsetData));
            if (returnObject.isSuccess) {
                this.events = returnObject.result;
            }
        },
        async callApi(path, requestObject) {
            let requestBoby = JSON.stringify(requestObject);
            try {
                let options = {
                    method: "POST",
                    headers: {
                        "Content-Type": "application/json",
                    },
                    body: requestBoby,
                };
                let response = await fetch(path, options);
                let data = await response.json();
                console.log(data);
                return data;
            } catch (err) {
                console.error(`前端出現錯誤，錯誤訊息：${err}`);
                return false;
            }
        },
    },
    mounted() {},
});

app.mount(".content");
