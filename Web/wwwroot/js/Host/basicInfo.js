
let save_btn = document.querySelector(".save-btn");

save_btn.addEventListener("click", () => {
    window.removeEventListener("beforeunload", beforeUnloadHandler) //以正規方法跳轉則移除事件
    window.location.assign("Description");
});

window.addEventListener('beforeunload', beforeUnloadHandler); //掛上事件

function beforeUnloadHandler(event) { //定義事件內容，若是用匿名函式無法取消監聽
    event.preventDefault();
    event.returnValue = "是否確定要離開網站?";
}
// 設定結束時間的最小值為開始時間
document.getElementById("basic-start-time").addEventListener("change", function () {
    document.getElementById("basic-end-time").min = this.value;
});

$(document).ready(function () {
    $('#image-input-imgur').change(function () {

        var formData = new FormData();
        var img = $("#image-input-imgur")[0].files[0];
        formData.append("file", img);
        console.log("submitBtn--imgur");

        $.ajax({
            url: '/Host/Upload', 
            type: 'POST',
            data: formData,
            processData: false,
            contentType: false,
            success: function (response) {
                console.log(response);
                var imgLink = response;
                $("#image-input-imgur-src").attr("src", imgLink);
                $("#Img").attr("value", imgLink);
            },
            error: function (xhr, status, error) {
                console.log(xhr.responseText);
            }
        });

    });
});





