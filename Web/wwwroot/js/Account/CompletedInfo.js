


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
    event.preventDefault();
    
});

//捨棄變更，重新整理頁面
document.querySelector('.button1').addEventListener('click', function () {
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


//驗證用IF，抽出來
function CallSwal(icon, title, text, targetDOM) {
    CallSwalFire(icon, title, text);
    targetDOM.classList.add('red-border');
    document.body.style.overflowY = 'auto';
    return;
};

function CallSwalFire(icon, title, text) {
    return Swal.fire({
        icon: icon,
        title: title,
        text: text,
        showConfirmButton: true,
        heightAuto: false,
        customClass: {
            title: 'S2_title_class',
            content: 'S2_content_class',
        }
    });
}

//儲存變更
document.getElementById('saveChanges').addEventListener('click', function (event) {
    event.preventDefault();
    document.body.style.overflowY = 'hidden';
    const name = document.querySelector('.realName').value.trim(); //姓名
    const phone = document.querySelector('.phone').value.trim(); //手機
    const enCnRegex = /^(?! )(?!.* $)[a-zA-Z\u4e00-\u9fa5 ]+$/; // 大小寫英文字母和中文格式(文字間可含空白)
    const phoneNumberRegex = /^09\d{8}$|^09\d{2}-\d{3}-\d{3}$|^09\d{2}-\d{6}$/; // 數字和減號格式
    const newPassword = document.querySelector('.newPassword').value.trim(); //新密碼
    const passwordRegex = /^[a-zA-Z0-9]+$/; //大小寫英文、數字格式
    
    let icon, title, text, targetDom;

    if (phone !== '' && phone.length < 10) {
        icon = 'warning';
        title = '【長度提示】';
        text = '「手機」的長度，不能少於10個字元。';
        targetDom = document.querySelector('.phone');
        CallSwal(icon, title, text, targetDom);
        return;
    }
    else if (phone !== '' && !phoneNumberRegex.test(phone)) {
        icon = 'warning'
        title = '【格式提示】';
        text = '「手機」的格式，需以09開頭，且僅能輸入數字和減號。(範例：09xxxxxxxx、09xx-xxx-xxx、09xx-xxxxxx)';
        targetDom = document.querySelector('.phone');
        CallSwal(icon, title, text, targetDom)
        return;
    }
    else if (name !== '' && !enCnRegex.test(name)) {
        icon = 'warning'
        title = '【格式提示】';
        text = '「姓名」的格式，僅能輸入大小寫英文或中文。';
        targetDom = document.querySelector('.realName');
        CallSwal(icon, title, text, targetDom)
        return;
    }
    else if (name === '') {
        icon = 'warning'
        title = '【必填項目提示】';
        text = '請輸入「姓名」。';
        targetDom = document.querySelector('.realName');
        CallSwal(icon, title, text, targetDom)
        return;
    }
    else if (phone === '') {
        icon = 'warning'
        title = '【必填項目提示】';
        text = '請輸入「手機號碼」。';
        targetDom = document.querySelector('.phone');
        CallSwal(icon, title, text, targetDom)
        return;
    }
    else if (newPassword !== '' && newPassword.length < 7) {
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
    else if (newPassword !== '' && !passwordRegex.test(newPassword)) {
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
    else if (newPassword === '') {
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

    document.querySelector('form').submit();
})