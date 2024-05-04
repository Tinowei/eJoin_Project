
function resetCheckbox() {
    var checkbox = document.getElementById('profile-container-switch');
    checkbox.checked = false;
}

window.addEventListener('resize', function () {
    resetCheckbox();
});

resetCheckbox();




//const menuSwitch = document.getElementById('menu-switch');
//const overlay = document.querySelector('.overlay');

//menuSwitch.addEventListener('change', function () {
 
//    if (menuSwitch.checked) {
//        overlay.style.display = 'block';
//    } else {
//        overlay.style.display = 'none';
//    }
//});


// 監聽螢幕大小變化
//window.addEventListener('resize', function () {

//    if (window.innerWidth <= 768) {
//        menuSwitch.checked = false;
//        overlay.style.display = 'none';
//    }
//});

//document.addEventListener('DOMContentLoaded', function () {
//    if (window.innerWidth <= 768) {
//        menuSwitch.checked = false;

//    }
//});