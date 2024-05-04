const { createApp } = Vue

createApp({
    data() {
        console.log(memberMyEventModel)
        return {
            trackGrid: memberMyEventModel.trackGrid,
            isCallUnFollow: false
        }
    },
    methods: {
        unFollow(memberId) {
            //console.log('[my evnet] unFollow: ', memberId)
            let self = this
            self.isCallUnFollow = true
            let followDto = {
                BeingFollowedId: memberId
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
                    self.trackGrid = self.trackGrid.filter(item => item.organizerId !== memberId)
                    self.isCallUnFollow = false
                })
                .fail(function (msg) {
                    console.error("Failed to unfollow:", msg);
                });
        }
    }
}).mount('#memberMyEventDiv')

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








