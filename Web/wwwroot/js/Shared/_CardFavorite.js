

$(function () {// 畫面載入完成時
    $('.tag-like').on('click', tagLikeClickEvent)
})

function tagLikeClickEvent(e) {
    let dom = e; // 為了搜尋頁面的vue特別寫的區段
    if (!e.nodeType) {
        dom = e.currentTarget; //把this改掉，換成事件的觸發元素
    }
    $(dom).css("pointer-events", "none");//防止重複點擊
    let heartCountElement = $(dom).find('.heart-count');
    let eventId = parseInt($(dom).attr("eventId"))


    if ($(dom).hasClass('liked')) {
        unLike(eventId, function (currentLikeCount) {
            $(dom).removeClass('liked');
            $(dom).find('.like-icon-img').show();
            $(dom).find('.like-fill-icon').hide();
            heartCountElement.text(currentLikeCount);

            let similarElements = $('.tag-like[eventId="' + eventId + '"]').not(dom);

            // 現在 similarElements 包含所有具有相同 eventId 但排除了當前被點擊的元素
            similarElements.each(function () {
                // 在這裡處理找到的相似元素
                let similarDom = this
                let similarHeartCountElement = $(similarDom).find('.heart-count');
                $(similarDom).removeClass('liked');
                $(similarDom).find('.like-icon-img').show();
                $(similarDom).find('.like-fill-icon').hide();
                similarHeartCountElement.text(currentLikeCount);
            });

            $(dom).css("pointer-events", "");// Enable click event

            if (typeof callRemoveLikeSuccess == 'function') {
                callRemoveLikeSuccess(eventId)
            }
        })
    } else {
        like(eventId, function (currentLikeCount) {
            $(dom).addClass('liked');
            $(dom).find('.like-icon-img').hide();
            $(dom).find('.like-fill-icon').show();
            heartCountElement.text(currentLikeCount);

            let similarElements = $('.tag-like[eventId="' + eventId + '"]').not(dom);

            // 現在 similarElements 包含所有具有相同 eventId 但排除了當前被點擊的元素
            similarElements.each(function () {
                // 在這裡處理找到的相似元素
                let similarDom = this
                let similarHeartCountElement = $(similarDom).find('.heart-count');
                $(similarDom).addClass('liked');
                $(similarDom).find('.like-icon-img').hide();
                $(similarDom).find('.like-fill-icon').show();
                similarHeartCountElement.text(currentLikeCount);
            });

            $(dom).css("pointer-events", "");// Enable click event

            if (typeof callAddLikeSuccess == 'function') {
                callAddLikeSuccess(eventId)
            }
        })
    }
}

function like(eventId, successCallBack) {
    let likeDto = {
        EventId: eventId
    }
    console.log(likeDto)
    $.ajax({
        method: "POST",
        url: "/api/Likes/Add",
        headers: {
            "Content-Type": "application/json"
        },
        data: JSON.stringify(likeDto),
        error: function (xhr, status, error) {
            if (xhr.status === 401) {
                window.location.href = "/Login"
            }
        }
    })
        .done(function (currentLikeCount) {
            console.log('like message :', currentLikeCount)
            successCallBack(currentLikeCount)
        })
        .fail(function (msg) {

        });
}

function unLike(eventId, successCallBack) {
    let likeDto = {
        EventId: eventId
    }
    $.ajax({
        method: "POST",
        url: "/api/Likes/Remove",
        headers: {
            "Content-Type": "application/json"
        },
        data: JSON.stringify(likeDto),
        error: function (xhr, status, error) {
            if (xhr.status === 401) {
                window.location.href = "/Login"
            }
        }
    })
        .done(function (currentLikeCount) {
            console.log('un like message :', currentLikeCount)
            successCallBack(currentLikeCount)
        })
        .fail(function (msg) {

        });
}