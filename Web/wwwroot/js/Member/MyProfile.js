
//回頂端
const arrow = document.querySelector('.top');  /*透過選擇器(.top)，取得DOM元素，放進變數arrow*/
window.addEventListener('scroll', () => {      /*針對「視窗」，新增「滾動」這個事件，進行監聽*/
    const scroll = window.scrollY;         /*將滾動視窗y軸這件事，放進變數scroll*/

    if (scroll > 0) {                           /*如果變數scroll(滾動視窗y軸)大於0*/
        arrow.classList.add('visible');         /*就在變數arrow的class，新增一個叫做visible的class*/
    } else {                                    /*否則移除叫做visible的class*/
        arrow.classList.remove('visible');
    }}
);


//一網頁有多分頁時，防止回頂端改變網址，造成被彈回預設頁面
document.addEventListener('DOMontentLoaded', function () {
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

// 抓取"你的性別"的三顆按鈕
const yourSexButtons = document.querySelectorAll('.yourSex');
// 抓取"你的感情狀態"的三顆按鈕
const yourRelationshipButtons = document.querySelectorAll('.yourRelationship');
// 預設目前選擇的按鈕
let selectedSexButton = document.querySelector('.yourSex.blue-background');
let selectedRelationshipButton = document.querySelector('.yourRelationship.blue-background');

yourSexButtons.forEach(button => {
    button.addEventListener('click', function (event) {
        event.preventDefault(); // 取消預設行為
        //取消先前選擇的按鈕
        if (selectedSexButton !== null && selectedSexButton !== button) {
            selectedSexButton.classList.remove('blue-background');
        }
        //切換按鈕的樣式
        button.classList.toggle('blue-background');
        selectedSexButton = button.classList.contains('blue-background') ? button : null;
    });
});

yourRelationshipButtons.forEach(button => {
    button.addEventListener('click', function (event) {
        event.preventDefault(); // 取消預設行為
        //取消先前選擇的按鈕
        if (selectedRelationshipButton !== null && selectedRelationshipButton !== button) {
            selectedRelationshipButton.classList.remove('blue-background');
        }
        //切換按鈕的樣式
        button.classList.toggle('blue-background');
        selectedRelationshipButton = button.classList.contains('blue-background') ? button : null;
    });
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

//個人簡介，字數統計
document.addEventListener('DOMContentLoaded', function () {
    const textarea = document.querySelector('.textarea');
    const count = document.querySelector('.textareaCount');
    const defaultValueLength = textarea.value.length;

    count.textContent = defaultValueLength + " / 255";

    textarea.addEventListener('input', function () {
        count.textContent = (defaultValueLength + textarea.value.length - textarea.defaultValue.length) + " / 255";
    });
});

/*更改值後，紅框消失*/
document.querySelectorAll('.realName,.displayName,.phone').forEach((input) => {
    input.addEventListener('input', function () {
        this.classList.remove('red-border');
    });
});



//驗證用IF，抽出來
function CallSwal(icon, title, text, targetDOM) {
    CallSwalFire(icon, title, text)
    targetDOM.classList.add('red-border');
    document.body.style.overflowY = 'auto';
    return;
}
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

//頭像圖和背景圖
const avatarImageFile = document.getElementById('avatarImageFile'); //頭像圖檔案上傳的input
const avatarPreviewImg = document.getElementById('avatarPreviewImg'); //頭像預覽圖片
const bgImageFile = document.getElementById('bgImageFile'); //背景圖檔案上傳的input
const bgPeviewImg = document.getElementById('bgPreviewImg');//背景圖預覽圖片

avatarImageFile.dataset.isChange = false; //頭像預設圖是否被改變
bgImageFile.dataset.isChange = false;   //背景預設圖是否被改變

avatarImageFile.addEventListener('change', function () {
    const file = this.files[0];
    console.log(file);
    if (file) {
        avatarImageFile.dataset.isChange = true
        DisplayImg(file, avatarPreviewImg);
    }
})
bgImageFile.addEventListener('change', function () {
    const file = this.files[0];
    if (file) {
        bgImageFile.dataset.isChange = true
        DisplayImg(file, bgPeviewImg);
    }
})


//圖片轉網址
function DisplayImg(file, previewImg) {
    let reader = new FileReader(); // 建立一個reader物件
    
    reader.addEventListener("load", function () {
        previewImg.src = reader.result;
    } )
    reader.readAsDataURL(file);// 讀取文件內容
}


//儲存變更
document.getElementById('saveChanges').addEventListener('click', function (event) {
    /*    event.preventDefault();*/
    document.body.style.overflowY = 'hidden';
    //姓名
    const name = document.querySelector('.realName').value.trim(); 
    //顯示姓名
    const displayName = document.querySelector('.displayName').value.trim(); 
    //手機
    const phone = document.querySelector('.phone').value.trim(); 
    //性別
    const genderElement = document.querySelector('.yourSex.blue-background');
    const gender = genderElement ? parseInt(genderElement.value) : 0; 
    //感情狀態
    const emotionalStateElement = document.querySelector('.yourRelationship.blue-background');
    const emotionalState = emotionalStateElement ? parseInt(emotionalStateElement.value) : 0; 
    //個人簡介
    const personalIntroductionElement = document.querySelector('.textarea');
    const personalIntroduction = personalIntroductionElement ? personalIntroductionElement.value.trim() : '';
    //詳細地址
    const detailAddressElement = document.querySelector('.address');
    const detailAddress = detailAddressElement ? detailAddressElement.value.trim() : '';
    //生日
    const birthdayElement = document.querySelector('.birthday');
    const birthday = birthdayElement ? birthdayElement.value : '';
    //現居地
    const cityElement = document.querySelector('.city');
    const city = cityElement ? cityElement.value.trim() : '';
    //驗證規則
    const enCnRegex = /^(?! )(?!.* $)[a-zA-Z\u4e00-\u9fa5 ]+$/; // 大小寫英文字母和中文格式(文字間可含空白)
    const phoneNumberRegex = /^09\d{8}$|^09\d{2}-\d{3}-\d{3}$|^09\d{2}-\d{6}$/; // 數字和減號格式

    let icon, title, text, targetDom;
    if (phone !== '' && phone.length < 10) {
        icon = 'warning'
        title = '【長度提示】';
        text = '「手機」的長度，不能少於10個字元。';
        targetDom = document.querySelector('.phone');
        CallSwal(icon, title, text, targetDom)
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
    else if (displayName !== '' && !enCnRegex.test(displayName)) {
        icon = 'warning'
        title = '【格式提示】';
        text = '「顯示名稱」的格式，僅能輸入大小寫英文或中文。';
        targetDom = document.querySelector('.displayName');
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
    else if (displayName === '') {
        icon = 'warning'
        title = '【必填項目提示】';
        text = '請輸入「顯示名稱」。';
        targetDom = document.querySelector('.displayName');
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

    // 1. 呼叫GetImageUrl(DOM)，並取得回傳值
    // 2. 之後拿回傳值判斷是不是有成功，如果失敗了就return結束按鈕的function；如果成功程式碼繼續執行
    let bgUrl = bgImageFile.dataset.default;
    let avatarUrl = avatarImageFile.dataset.default;
    GetImageUrl(bgImageFile).then(result => {
        if (!result) {
            icon = 'warning'
            title = '【上傳背景圖失敗】';
            text = '請檢查檔案格式是否為.jpg、.jpeg、.png、.webp，若需要幫助，請聯繫「你揪你維護小組」(該圖外的資料還是會為您儲存)';
            return CallSwalFire(icon, title, text).then((result) => {
                if (result.isConfirmed) {
                    return;
                }
            });
        }
        bgUrl = result;
        console.log(bgUrl)
    }).then(() => {
        GetImageUrl(avatarImageFile).then(result => {
            if (!result) {
                if (!result) {
                    icon = 'warning'
                    title = '【上傳頭像失敗】';
                    text = '請檢查檔案格式是否為.jpg、.jpeg、.png、.webp，若需要幫助，請聯繫「你揪你維護小組」(該圖外的資料還是會為您儲存)';
                    return CallSwalFire(icon, title, text).then((result) => {
                        if (result.isConfirmed) {
                            return;
                        }
                    });
                }
            }
            avatarUrl = result;
            console.log(avatarUrl)
        }).then(() => {
            fetch('/api/Member/GetMemberProfile', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json'
                },
                body: JSON.stringify({
                    name: name,
                    displayName: displayName,
                    phone: phone,
                    personalIntroduction: personalIntroduction,
                    detailAddress: detailAddress,
                    AvatarUrl: avatarUrl,
                    BackgroundUrl: bgUrl,
                    Birthday: birthday,
                    Gender: gender,
                    EmotionalState: emotionalState,
                    City: city,
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
                                icon = 'warning';
                                title = '【必填項目提示】';
                                text = '請輸入「姓名」';
                                CallSwalFire(icon, title, text)
                                break;
                            case 2:
                                icon = 'warning';
                                title = '【必填項目提示】';
                                text = '請輸入「顯示名稱」。';
                                CallSwalFire(icon, title, text)
                                break;
                            case 3:
                                icon = 'warning';
                                title = '【必填項目提示】';
                                text = '請輸入「手機號碼」。';
                                CallSwalFire(icon, title, text)
                                break;
                            case 4:
                                icon = 'warning';
                                title = '【長度提示】';
                                text = '「手機」的長度，不能少於10個字元。';
                                CallSwalFire(icon, title, text)
                                break;
                            case 5:
                                icon = 'warning';
                                title = '【長度提示】';
                                text = '「手機」的長度，不能多於12個字元。';
                                CallSwalFire(icon, title, text)
                                break;
                            case 6:
                                icon = 'warning';
                                title = '【長度提示】';
                                text = '「姓名」的長度，不能多於50個字元。';
                                CallSwalFire(icon, title, text)
                                break;
                            case 7:
                                icon = 'warning';
                                title = '【長度提示】';
                                text = '「顯示姓名」的長度，不能多於100個字元。';
                                CallSwalFire(icon, title, text)
                                break;
                            case 8:
                                icon = 'warning';
                                title = '【長度提示】';
                                text = '「個人簡介」的長度，不能多於250個字元。';
                                CallSwalFire(icon, title, text)
                                break;
                            case 9:
                                icon = 'warning';
                                title = '【長度提示】';
                                text = '「詳細地址」的長度，不能多於200個字元。';
                                CallSwalFire(icon, title, text)
                                break;
                            case 10:
                                icon = 'warning';
                                title = '【格式提示】';
                                text = '需以09開頭，且僅能輸入數字和減號。(範例：09xxxxxxxx、09xx-xxx-xxx、09xx-xxxxxx)';
                                CallSwalFire(icon, title, text)
                                break;
                            case 11:
                                icon = 'warning';
                                title = '【格式提示】';
                                text = '「姓名」的格式，僅能輸入大小寫英文或中文。';
                                CallSwalFire(icon, title, text)
                                break;
                            case 12:
                                icon = 'warning';
                                title = '【格式提示】';
                                text = '「顯示名稱」的格式，僅能輸入大小寫英文或中文。';
                                CallSwalFire(icon, title, text)
                                break;
                            case 13:
                                icon = 'success';
                                title = '【儲存成功!】';
                                text = '您的個人資料已成功變更。';
                                CallSwalFire(icon, title, text).then((result) => {
                                    if (result.isConfirmed) {
                                        location.reload(); // 重新整理頁面
                                    }
                                });
                                break;
                            default:
                                icon = 'warning';
                                title = '【儲存失敗提示】';
                                text = '發生未知的錯誤，請聯繫你揪你維修小組。';
                                CallSwalFire(icon, title, text)
                                break;
                        }
                    }
                })
                .catch(error => {
                    console.error('There was a problem with your fetch operation:', error);
                });
        })        
    })

});

//確認圖片，是否被改變過
async function GetImageUrl(element) {
    console.log(element.dataset.isChange)
    if (element.dataset.isChange == 'false') { //如果圖片沒有被改變過，回傳原本的圖，否則繼續往下執行
        return element.dataset.default;
    }
    let formData = new FormData; //建立一個放資料的formData物件
    formData.append('file', element.files[0]); //抓第一張圖
    let ChooseApi = element.files.length === 1 ? '/api/Upload/UploadImage' : '/api/Upload/UploadImages'; //判斷圖片是否為一張，並回傳路由
    let options = { method: 'POST', body: formData }; //建立一個post方法，內容是formData物件
    try {
        let response = await fetch(ChooseApi, options);
        let data = await response.json();

        if (data.error) {
            //icon = 'error';
            //title = '【上傳圖片提示】';
            //text = '圖片上傳失敗!' + data.error;
            //return CallSwalFire(icon, title, text).then((result) => {
            //    if (result.isConfirmed) {
            //        return false;
            //    }
            //});
            return false;

        } else {
            console.log(data);
            console.log('用的ChooseApi是', ChooseApi);
            //icon = 'success';
            //title = '【上傳圖片提示】';
            //text = '圖片上傳成功!'; //圖片網址為：' + data.url
            //return CallSwalFire(icon, title, text).then((result) => {
            //    if (result.isConfirmed) {
            //        return data.url;
            //    }
            //});

            return data.url;

        }
    } catch (error) {
        //icon = 'error';
        //title = '【錯誤提示】';
        //text ='上傳圖片發生不明錯誤，請聯繫你揪你維護小組。' ; //error
        //return CallSwalFire(icon, title, text).then((result) => {
        //    if (result.isConfirmed) {
        //        return false;
        //    }
        //});

        return false;
    }
}


//================= 載入頁面時生效(換方法)//=================
//window.addEventListener('load', function () {
//    fetch('/api/Member/GetMemberProfileImage')
//        .then(response => {
//            if (!response.ok) {
//                throw new Error('Network response was not ok');
//            }
//            return response.json();
//        })
//        .then(data => {
//            if (data && data.avatarPeviewUrl) {
//                avatarPreviewImg.src = data.AvatarPeviewUrl;
//            } else {
//                avatarPreviewImg.src = "~/images/profilePicture.png"; // 預設頭像圖片 URL
//            }

//            if (data && data.backgroundPeviewUrl) {
//                bgPeviewImg.src = data.BackgroundPeviewUrl;
//            } else {
//                bgPeviewImg.src = "~/images/cardUploadImg.png"; // 預設背景圖片 URL
//            }
//        })
//        .catch(error => {
//            console.error('There was a problem with your fetch operation:', error);
//        });
//});


//=================圖上傳後顯示在畫面上(換方法)=================
//function previewImage(input, targetSelector) {
//    const file = input.files[0];
//    const reader = new FileReader();
//    reader.onload = function (e) {
//        $(targetSelector).attr('src', e.target.result);
//    };
//    reader.readAsDataURL(file);
//}
//$('#bgImg').change(function () {
//    previewImage(this, '.backgroundImgOuter img');
//});
//$('#headImg').change(function () {
//    previewImage(this, '.avatar-preview img');
//});


//=================以下3/31註解 by舒舒=================
//test by tino
/*
const imageFileInput = document.getElementById('imageFile');
const previewImg = document.getElementById('previewImg');

imageFileInput.addEventListener('change', function () {
    const file = this.files[0];
    console.log(file);
    if (file) {
        DisplayImage(file);
    }
    //CallUploadApi();  
});

//display Image in backgroundImgOuter
function DisplayImage(file) {
    let reader = new FileReader();
    reader.onload = function (e) {
        // 更新圖片預覽的src屬性
        previewImg.src = e.target.result;
    };
    // 讀取文件內容
    reader.readAsDataURL(file);
}

//api
async function CallUploadApi() {
    let formData = new FormData(); 
    const fileInput = document.getElementById('imageFile');
    formData.append('file', fileInput.files[0]);
    let ChooseApi = imageFileInput.files.length === 1 ? '/api/Upload/UploadImage' : '/api/Upload/UploadImages';
    let options = { method: 'POST', body: formData };

    try {
        let response = await fetch(ChooseApi, options);
        let data = await response.json();

        if (data.error) {
            alert('上傳失敗：' + data.error);
        } else {
            console.log(data);
            console.log('用的ChooseApi是', ChooseApi);
            alert('上傳成功！圖片URL：' + data.url);
            return data.url;
        }
    } catch (error) {
        alert('發生錯誤：' + error);
        return false;
    }
}




*/