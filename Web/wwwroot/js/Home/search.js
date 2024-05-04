function openFilterAreaPopup() {
    document.getElementById("filter-area-popup").style.display = "block";
    const overlay = $(".filter-area-overlay");
    overlay.show();
}

function closeFilterAreaPopup() {
    document.getElementById("filter-area-popup").style.display = "none";
    const overlay = $(".filter-area-overlay");
    overlay.hide();
    console.log("hidden !!");
}

function showLocateArea() {
    document.getElementById("choose-locate-area").style.display = "block";
}

function hideLocateArea() {
    document.getElementById("choose-locate-area").style.display = "none";
}

let filterDateRangeStatus = {
    isPicker: false,
};

$(function () {
    $('input[name="daterange"]').daterangepicker(
        {
            opens: "left",
            locale: {
                format: "YYYY/MM/DD",
                separator: "-",
                applyLabel: "確認",
                cancelLabel: "取消",
                fromLabel: "從",
                toLabel: "到",
                customRangeLabel: "Custom",
                weekLabel: "W",
                daysOfWeek: ["Sun", "Mon", "Tue", "Wed", "Thur", "Fri", "Sat"],
                monthNames: [
                    "January",
                    "February",
                    "March",
                    "April",
                    "May",
                    "June",
                    "July",
                    "August",
                    "September",
                    "October",
                    "November",
                    "December",
                ],
                firstDay: 1,
            },
        },
        function (start, end, label) {
            console.log(
                "A new date selection was made: " + start.format("YYYY-MM-DD") + " to " + end.format("YYYY-MM-DD")
            );
        }
    );
});

const { createApp } = Vue;

const app = createApp({
    data() {
        return {
            queryStringObj: { //todo
                keyword: null,
                selectedOrderBy: null,
                selectedPrice: null,
                selectedTime: null,
                selectedThemes: [],
                selectedPlaces: [],
            },
            requsetData: {
                keyword: null,
                selectedOrderBy: "focus",
                selectedPrice: null,
                selectedTime: null,
                selectedThemes: [],
                selectedPlaces: [],
                currentPage: 1,
            }, 
            events: [
                //{
                    //eventId: 1,
                    //eventCoverUrl: "https://res.cloudinary.com/dlhaqaap3/image/upload/v1712025610/ImageUploadByApi/tu8utesrztfffboqteli.jpg",
                    //eventStartDate: "2024-04-09",
                    //eventEndDate: "2024-05-01",
                    //eventTitle: "Tech Conference1",
                    //eventCity: "台北市",
                    //eventThemes: [
                    //    "公益", "科技"
                    //],
                    //isLike: true,
                    //heartCount: 5,
                //}
            ],
            load: true,
        };
    },
    methods: {
        orderBySelectOnChange(e) {
            this.queryStringObj.selectedOrderBy = e.currentTarget.value;
            this.display();
        },
        priceBtnOnclick(e) {
            if (e.currentTarget.classList.contains("checked")) {
                e.currentTarget.classList.remove("checked");
                this.queryStringObj.selectedPrice = null;
                return;
            }
            this.queryStringObj.selectedPrice = e.currentTarget.dataset.price;
            this.display();
        },
        timeBtnOnclick(e) {
            if (e.currentTarget.classList.contains("checked")) {
                e.currentTarget.classList.remove("checked");
                this.queryStringObj.selectedTime = null;
                return;
            }
            this.queryStringObj.selectedTime = e.currentTarget.dataset.time;
            this.display();
        },
        themeBtnOnclick(e) {
            if (e.currentTarget.classList.contains("checked")) {
                e.currentTarget.classList.remove("checked");
                this.queryStringObj.selectedThemes = this.queryStringObj.selectedThemes.filter((theme) => theme != e.currentTarget.dataset.themeId);
                this.display();
                return;
            }
            this.queryStringObj.selectedThemes.push(e.currentTarget.dataset.themeId);
            e.currentTarget.classList.add("checked");
            this.display();
        },
        placeBtnOnclick(e) {
            if (e.currentTarget.classList.contains("checked")) {
                e.currentTarget.classList.remove("checked");
                this.queryStringObj.selectedPlaces = this.queryStringObj.selectedPlaces.filter((place) => place != e.currentTarget.dataset.place);
                this.display();
                return;
            }

            this.queryStringObj.selectedPlaces.push(e.currentTarget.dataset.place);
            e.currentTarget.classList.add("checked");
            this.display();
        },
        searchBtnOnclick() {
            this.queryStringObj.keyword = document.querySelector("#keyword-input").value;
            let url = `/Home/Search${this.getQueryString()}`
            console.log(url);
            window.location.assign(url);
            closeFilterAreaPopup();

        },
        heartBtnOnclick(event) {
            tagLikeClickEvent(event.currentTarget);
        },
        display() {
            console.log(this.queryStringObj);
        },
        async sentFillter() {
            let apiPath = "/api/Search"
            let returnObject = await (this.callApi(apiPath, this.requsetData));
            if (returnObject.isSuccess == true) {
                returnObject.result.forEach(e => {
                    this.events.push(e);
                })
            }
            if (returnObject.message == "end") {
                this.load = false;
            }
            this.requsetData.currentPage = this.requsetData.currentPage + 1;
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
        getQueryStringAsObject() {
            let queryString = window.location.search.substring(1); // 移除開頭的 "?"
            let pairs = decodeURIComponent(queryString).split('&'); // 將查詢字串分割成鍵值對
            let result = {};

            pairs.forEach(function (pair) {
                let keyValue = pair.split('='); // 將每個鍵值對分割成鍵和值
                let key = keyValue[0];
                let value = keyValue[1];

                // 對於 selectedThemes 和 selectedPlaces，將值分割為數組
                if (key === 'selectedThemes' || key === 'selectedPlaces') {
                    result[key] = value ? value.split('%2C') : [];
                } else {
                    result[key] = value;
                }
            });

            console.log(result)
            return result;
        },
        getQueryString() {
            const params = new URLSearchParams();
            if (this.queryStringObj.keyword) params.append('keyword', this.queryStringObj.keyword);
            if (this.queryStringObj.selectedOrderBy !== null) params.append('selectedOrderBy', this.queryStringObj.selectedOrderBy);
            if (this.queryStringObj.selectedPrice !== null) params.append('selectedPrice', this.queryStringObj.selectedPrice);
            if (this.queryStringObj.selectedTime !== null) {
                if (this.queryStringObj.selectedTime == 'customize') this.queryStringObj.selectedTime = this.$refs.dateRange.value;
                params.append('selectedTime', this.queryStringObj.selectedTime);
            }
            if (this.queryStringObj.selectedThemes && this.queryStringObj.selectedThemes.length > 0) params.append('selectedThemes', this.queryStringObj.selectedThemes.join(','));
            if (this.queryStringObj.selectedPlaces && this.queryStringObj.selectedPlaces.length > 0) params.append('selectedPlaces', this.queryStringObj.selectedPlaces.join(','));

            const queryString = params.toString();
            return queryString ? `?${queryString}` : '';
        },
        observerCallback(entries) {
            entries.forEach((entry) => {
                if (entry.isIntersecting) {
                    this.sentFillter();
                }
            });
        },
    },
    mounted() {
        const queryParams = this.getQueryStringAsObject();
        const allowedKeys = Object.keys(this.requsetData);

        // 重置 requsetData 為初始值
        Object.assign(this.requsetData, this.$options.data().requsetData);

        // 只更新 requsetData 中允許的鍵
        allowedKeys.forEach(key => {
            if (queryParams[key] !== undefined) {
                if (key === 'selectedThemes' || key === 'selectedPlaces') {
                    this.requsetData[key] = queryParams[key] ? queryParams[key] : [];
                } else {
                    this.requsetData[key] = queryParams[key];
                }
            }
        });
        this.display();

        const observer = new IntersectionObserver(this.observerCallback);
        observer.observe(this.$refs.loading);
    },
});

app.mount(".content");
