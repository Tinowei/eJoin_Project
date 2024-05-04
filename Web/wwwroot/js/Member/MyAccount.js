
//當已經在頂端，回頂端符號隱藏
const arrow = document.querySelector('.top');  /*透過選擇器(.top)，取得DOM元素，放進變數arrow*/
window.addEventListener('scroll', () =>     /*針對「視窗」，新增「滾動」這個事件，進行監聽*/ {
    const scroll = window.scrollY;         /*將滾動視窗y軸這件事，放進變數scroll*/

    if (scroll > 0) {                           /*如果變數scroll(滾動視窗y軸)大於0*/
        arrow.classList.add('visible');         /*就在變數arrow的class，新增一個叫做visible的class*/
    } else {                                    /*否則移除叫做visible的class*/
        arrow.classList.remove('visible');
    }
}
);

//一網頁有多分頁時，防止回頂端改變網址，造成被彈回預設頁面
document.addEventListener('DOMContentLoaded', function () {
    document.body.style.overflow = 'hidden';
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



//捨棄變更，重新整理頁面
document.querySelector('.button1').addEventListener('click', function () {
    event.preventDefault(); // 防止表單自動提交
    document.body.style.overflow = 'hidden';
    Swal.fire({
        icon: 'warning',
        title: "【捨棄變更確認】",
        text: '點擊「確定」，將捨棄您尚未儲存的變更',
        confirmButtonText: "確定",
        showCancelButton: true,
        cancelButtonText: "取消",
        heightAuto: false,
        customClass: {
            title: 'S2_title_class',
            content: 'S2_content_class',
        }
    }).then((result) => {
        if (result.isConfirmed) {
            location.reload(); // 重新整理頁面
        }
    });
});

////規範(S2版)
document.getElementById('saveChanges').addEventListener('click', function (event) {
/*    event.preventDefault();*/
    const newPassword = document.querySelector('.newPassword').value.trim(); //新密碼
    const checkPassword = document.querySelector('.checkPassword').value.trim(); //確認密碼
    const originalPassword = document.querySelector('.originalPassword').value.trim(); //原本密碼
    const passwordRegex = /^[a-zA-Z0-9]+$/; //大小寫英文、數字格式

    if (newPassword !== '' && newPassword.length < 7) {
        Swal.fire({
            icon: 'warning',
            title: '【長度提示】',
            text: '「新密碼」的長度，不能少於7個字元。',
            showConfirmButton: true,
            heightAuto: false,
            customClass: {
                title: 'S2_title_class',
                content: 'S2_content_class',
            },
        });
        document.querySelector('.newPassword').classList.add('red-border');
        return;
    }

    if (checkPassword !== '' && checkPassword.length < 7) {
        Swal.fire({
            icon: 'warning',
            title: '【長度提示】',
            text: '「確認密碼」的長度，不能少於7個字元。',
            showConfirmButton: true,
            heightAuto: false,
            customClass: {
                title: 'S2_title_class',
                content: 'S2_content_class',
            }
        });
        document.querySelector('.checkPassword').classList.add('red-border');
        return;
    }

    if (newPassword !== '' &&!passwordRegex.test(newPassword)) {
        Swal.fire({
            icon: 'warning',
            title: '【格式提示】',
            text: '「新密碼」的格式，僅能輸入大小寫英文或數字。',
            showConfirmButton: true,
            heightAuto: false,
            customClass: {
                title: 'S2_title_class',
                content: 'S2_content_class',
            }
        });
        document.querySelector('.newPassword').classList.add('red-border');
        return;
    }

    if (checkPassword !== '' &&!passwordRegex.test(checkPassword)) {
        Swal.fire({
            icon: 'warning',
            title: '【格式提示】',
            text: '「確認密碼」的格式，僅能輸入大小寫英文或數字。',
            showConfirmButton: true,
            heightAuto: false,
            customClass: {
                title: 'S2_title_class',
                content: 'S2_content_class',
            }
        });
        document.querySelector('.checkPassword').classList.add('red-border');
        return;
    }

    if (newPassword !== '' && checkPassword !== ''&& newPassword !== checkPassword) {
        Swal.fire({
            icon: 'warning',
            title: '【密碼檢核提示】',
            text: '輸入的「新密碼」與「確認密碼」不一致。',
            showConfirmButton: true,
            heightAuto: false,
            customClass: {
                title: 'S2_title_class',
                content: 'S2_content_class',
            }
        });
        document.querySelector('.checkPassword').classList.add('red-border');
        return;
    }

    if (originalPassword === '') {
        Swal.fire({
            icon: 'warning',
            title: '【必填項目提示】',
            text: '請輸入「原本密碼」。',
            showConfirmButton: true,
            heightAuto: false,
            customClass: {
                title: 'S2_title_class',
                content: 'S2_content_class',
            }
        });
        document.querySelector('.originalPassword').classList.add('red-border');
        return;
    }

    if (newPassword === '') {
        Swal.fire({
            icon: 'warning',
            title: '【必填項目提示】',
            text: '請輸入「新密碼」。',
            showConfirmButton: true,
            heightAuto: false,
            customClass: {
                title: 'S2_title_class',
                content: 'S2_content_class',
            }
        });
        document.querySelector('.newPassword').classList.add('red-border');
        return;
    }

    if (checkPassword === '') {
        Swal.fire({
            icon: 'warning',
            title: '【必填項目提示】',
            text: '請輸入「確認密碼」。',
            showConfirmButton: true,
            heightAuto: false,
            customClass: {
                title: 'S2_title_class',
                content: 'S2_content_class',
            }
        });
        document.querySelector('.checkPassword').classList.add('red-border');
        return;
    }

    fetch('/api/Member/GetMemberPassword', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json'
        },
        body: JSON.stringify({
            newPassword: newPassword,
            originalPassword: originalPassword,
            checkPassword: checkPassword
        })
    })
        .then(response => {
            if (!response.ok) {
                throw new Error('Network response was not ok');
            }
            return response.json();
        })
        .then(data => {
            console.log(data)
            if (data && data.code) {
                switch (data.code) {

                    case 1:
                        Swal.fire({
                            icon: 'warning',
                            title: '【密碼檢核提示】',
                            text: '輸入的「原本密碼」不正確。',
                            showConfirmButton: true,
                            heightAuto: false,
                            customClass: {
                                title: 'S2_title_class',
                                content: 'S2_content_class',
                            }
                        });
                        break;
                    case 2:
                        Swal.fire({
                            icon: 'warning',
                            title: '【密碼檢核提示】',
                            text: '輸入的「新密碼」與「確認密碼」不一致。',
                            showConfirmButton: true,
                            heightAuto: false,
                            customClass: {
                                title: 'S2_title_class',
                                content: 'S2_content_class',
                            }
                        });
                        break;
                    case 3:
                        Swal.fire({
                            icon: 'warning',
                            title: '【長度提示】',
                            text: '「新密碼」的長度，不能少於7個字元。',
                            showConfirmButton: true,
                            heightAuto: false,
                            customClass: {
                                title: 'S2_title_class',
                                content: 'S2_content_class',
                            }
                        });
                        break;
                    case 4:
                        Swal.fire({
                            icon: 'warning',
                            title: '【長度提示】',
                            text: '「新密碼」的長度，不能多於30個字元。',
                            showConfirmButton: true,
                            heightAuto: false,
                            customClass: {
                                title: 'S2_title_class',
                                content: 'S2_content_class',
                            }
                        });
                        break;
                    case 5:
                        Swal.fire({
                            icon: 'warning',
                            title: '【格式提示】',
                            text: '「新密碼」的格式，僅能輸入大小寫英文和數字。',
                            showConfirmButton: true,
                            heightAuto: false,
                            customClass: {
                                title: 'S2_title_class',
                                content: 'S2_content_class',
                            }
                        });
                        break;
                    case 6:
                        Swal.fire({
                            icon: 'warning',
                            title: '【必填項目提示】',
                            text: '請輸入「原本密碼」。',
                            showConfirmButton: true,
                            heightAuto: false,
                            customClass: {
                                title: 'S2_title_class',
                                content: 'S2_content_class',
                            }
                        });
                        break;
                    case 7:
                        Swal.fire({
                            icon: 'warning',
                            title: '【必填項目提示】',
                            text: '請輸入「新密碼」。',
                            showConfirmButton: true,
                            heightAuto: false,
                            customClass: {
                                title: 'S2_title_class',
                                content: 'S2_content_class',
                            }
                        });
                        break;
                    case 8:
                        Swal.fire({
                            icon: 'warning',
                            title: '【必填項目提示】',
                            text: '請輸入「確認密碼」。',
                            showConfirmButton: true,
                            heightAuto: false,
                            customClass: {
                                title: 'S2_title_class',
                                content: 'S2_content_class',
                            }
                        });
                        break;
                    case 9:
                        Swal.fire({
                            icon: 'success',
                            title: '【儲存成功!】',
                            text: '您的密碼已成功變更。',
                            showConfirmButton: true,
                            heightAuto: false,
                            customClass: {
                                title: 'S2_title_class',
                                content: 'S2_content_class',
                            }
                        }).then((result) => {
                            if (result.isConfirmed) {
                                location.reload(); // 重新整理頁面
                            }
                        });
                        break;
                    default:
                        Swal.fire({
                            icon: 'warning',
                            title: '【儲存失敗提示】',
                            text: '發生未知的錯誤，請聯繫你揪你維修小組。',
                            showConfirmButton: true,
                            heightAuto: false,
                            customClass: {
                                title: 'S2_title_class',
                                content: 'S2_content_class',
                            }
                        });
                        break;
                }
            } 
        })
        .catch(error => {
            console.error('There was a problem with your fetch operation:', error);
        });
});



/*更改值後，紅框消失*/
document.querySelectorAll('.newPassword, .checkPassword, .originalPassword').forEach((input) => {
    input.addEventListener('input', function () {
        this.classList.remove('red-border');
    });
});



//顯示和隱藏密碼
document.addEventListener('DOMContentLoaded', function () {
    const passwordAndIconContainers = document.querySelectorAll('.passwordAndIcon');

    passwordAndIconContainers.forEach(container => {
        const passwordInput = container.querySelector('input[type="password"]');
        const closeEyeIcon = container.querySelector('.closeEye');
        const openEyeIcon = container.querySelector('.openEye');

        closeEyeIcon.addEventListener('click', function () {
            passwordInput.type = 'text';
            closeEyeIcon.style.display = 'none';
            openEyeIcon.style.display = 'block';
        });

        openEyeIcon.addEventListener('click', function () {
            passwordInput.type = 'password';
            closeEyeIcon.style.display = 'block';
            openEyeIcon.style.display = 'none';
        });
    });
});










