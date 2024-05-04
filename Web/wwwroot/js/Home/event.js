//對活動按心/收藏
$(function () {// 畫面載入完成時
    $('.owl-carousel').owlCarousel({
        loop: false,
        margin: 25,
        autoplay: false, //預設值為false，可自動輪播切換圖片
        nav: true, //顯示上下按鈕
        responsiveClass: true,
        responsive: {
            0: {
                items: 1,
                autoWidth: false,
            },
            700: {
                items: 2
            },
            1000: {
                items: 3,
                autoWidth: false
            }
        }
    })

    //$('.like-icon').on('click', function () {
    //    let heartCountElement = $(this).find('.heart-count');
    //    let heartCount = 0;

    //    if ($(this).hasClass('liked')) {
    //        $(this).removeClass('liked');
    //        $(this).find('.like-icon-img').show();
    //        $(this).find('.like-fill-icon').hide();
    //    } else {
    //        $(this).addClass('liked');
    //        $(this).find('.like-icon-img').hide();
    //        $(this).find('.like-fill-icon').show();
    //    }

    //    //TODO 發Api取得最新的愛心數量

    //    heartCountElement.text(heartCount);
    //})

    let card_list = $(".my-card-list")
    for (let i = 0; i < card_list.length; i++) {
        card_list[i].classList.add("item");
    }
})

const { markRaw } = Vue;

const mapApp = Vue.createApp({
    data() {
        return {
            lat: "",
            lng: "",
            map: null,
            marker: null,
        };
    },
    created() {
        this.city = city || "";
        this.address = address || "";
        this.lat = lat || "";
        this.lng = lng || "";
    },
    methods: {
        setMarker(lat, lng) {

            const newMarker = markRaw(L.marker([lat, lng]));

            markRaw(newMarker.bindPopup(`<b>${this.city}${this.address}</b>`));

            markRaw(newMarker.addTo(this.map));

            this.marker = newMarker;
        },
        initMap() {
            this.map = markRaw(L.map('map').setView([this.lat, this.lng], 18));

            markRaw(L.tileLayer('https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png', { minZoom: 8, maxZoom: 19 })
                .addTo(this.map));
        },
    },
    mounted() {
        // 初始化地圖
        this.initMap();

        // 在預設的位置上設置marker
        this.setMarker(this.lat, this.lng);
    }
}).mount(".activity-map-area");