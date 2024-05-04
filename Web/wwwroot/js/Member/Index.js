const { createApp } = Vue

createApp({
    data() {
        console.log(memberIndexViewModel)
        return {
            memberId: memberIndexViewModel.memberId,
            memberName: memberIndexViewModel.memberName,
            fansCount: memberIndexViewModel.fansCount,
            isFollow: memberIndexViewModel.isFollow,
            isCallTrackHost: false
        }
    },
    mounted() { //頁面載入完成 並 完成Vue綁定後
        this.getFansCount()
    },
    methods: {
        trackHost: function () {
            this.isCallTrackHost = true
            if (this.isFollow) {
                // 發Api取消追蹤
                this.unFollow();
            } else {
                // 發Api追蹤
                this.follow();
            }
        },
        follow() {
            //將vue的this本身存到self變數
            let self = this
            let followDto = {
                BeingFollowedId: this.memberId
            }
            $.ajax({
                method: "POST",
                url: "/api/Follow/Add",
                headers: {
                    "Content-Type": "application/json"
                },
                data: JSON.stringify(followDto),
                error: function (xhr, status, error) {
                    if (xhr.status === 401) {
                        window.location.href = "/Login"
                    }
                }
            })
                .done(function (msg) {
                    self.isFollow = true
                    self.getFansCount()
                    self.isCallTrackHost = false
                })
        },
        unFollow() {
            let self = this
            let followDto = {
                BeingFollowedId: this.memberId
            }
            $.ajax({
                method: "POST",
                url: "/api/Follow/Remove",
                headers: {
                    "Content-Type": "application/json"
                },
                data: JSON.stringify(followDto),
                error: function (xhr, status, error) {
                    if (xhr.status === 401) {
                        window.location.href = "/Login"
                    }                 
                }
            })
                .done(function (msg) {
                    self.isFollow = false
                    self.getFansCount()
                    self.isCallTrackHost = false
                })
        },
        getFansCount() {
            let self = this
            $.ajax({
                method: "GET",
                url: "/api/Follow/GetFansCount?memberId=" + this.memberId,
                headers: {
                    "Content-Type": "application/json"
                }
            })
                .done(function (fansCount) {
                    self.fansCount = fansCount
                })
                .fail(function (msg) {

                });
        }
    }
}).mount('#memberIndexDiv')

// banner縮放
window.addEventListener('scroll', function () {
    const userBanner = document.querySelector('.user-banner');
    const scrollPosition = document.body.scrollTop;
    const windowHeight = window.innerHeight;

    const scale = 1 + scrollPosition / windowHeight;
    const opacity = 1 - scrollPosition / windowHeight;

    userBanner.style.transform = `scale(${scale})`;
    userBanner.style.opacity = opacity;

    // 如果放大比例大於1，則隱藏視窗的水平捲軸，否則顯示
    if (scale > 1) {
        document.body.style.overflowX = 'hidden';
    } else {
        document.body.style.overflowX = 'auto';
    }
}, true);

document.querySelectorAll('#tab-list div').forEach(function (tab) {
    tab.addEventListener('click', function () {
        document.querySelectorAll('#tab-list div').forEach(function (otherTab) {
            otherTab.classList.remove('active');
        });
        tab.classList.add('active');
    });
});

//按按鈕移動矩形區塊
// document.addEventListener("DOMContentLoaded", function () {
//     const aboutTab = document.querySelector('.tab-list-about');
//     const tabLine = document.querySelector('.tab-line');

//     const aboutTabOffsetLeft = aboutTab.offsetLeft;
//     tabLine.style.transform = `translateX(${aboutTabOffsetLeft}px)`;

//     aboutTab.addEventListener('click', () => {
//         const aboutTabOffsetLeft = aboutTab.offsetLeft;
//         // 移動時添加效果
//         tabLine.style.transition = 'transform 0.3s ease-in-out';
//         tabLine.style.transform = `translateX(${aboutTabOffsetLeft}px)`;
//     });

//     const eventTab = document.querySelector('.tab-list-event');
//     eventTab.addEventListener('click', () => {
//         const eventTabOffsetLeft = eventTab.offsetLeft;
//         const eventTabWidth = eventTab.offsetWidth;
//         // 移動時添加效果
//         tabLine.style.transition = 'transform 0.3s ease-in-out';
//         tabLine.style.transform = `translateX(${eventTabOffsetLeft}px)`;
//         tabLine.style.width = `${eventTabWidth}px`;
//     });
// });

//切換頁面
function showAbout() {
    document.getElementById('area-about').style.display = 'block';
    document.getElementById('area-event').style.display = 'none';
}

function showAction() {
    document.getElementById('area-about').style.display = 'none';
    document.getElementById('area-event').style.display = 'block';
}


