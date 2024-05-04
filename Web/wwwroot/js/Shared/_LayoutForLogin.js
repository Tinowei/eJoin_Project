let checkbox = document.getElementById('profile-container-switch');
let photoContainer = document.querySelector('.photo-container img');
function resetCheckbox() {
    checkbox.checked = false;
}

window.addEventListener('resize', function () {
    resetCheckbox();
});

resetCheckbox();




const menuSwitch = document.getElementById('menu-switch');
const overlay = document.querySelector('.overlay');

menuSwitch.addEventListener('change', function () {
 
    if (menuSwitch.checked) {
        overlay.style.display = 'block';
    } else {
        overlay.style.display = 'none';
    }
});


// 監聽螢幕大小變化
window.addEventListener('resize', function () {

    if (window.innerWidth <= 768) {
        menuSwitch.checked = false;
        overlay.style.display = 'none';
    }
});

document.addEventListener('DOMContentLoaded', function () {
    if (window.innerWidth <= 768) {
        menuSwitch.checked = false;

    }
});

//點擊任何地方就把側邊選單收合起來
overlay.addEventListener('click', function () {
    menuSwitch.checked = false;
    overlay.style.display = "none";
})

const latout_search_input = document.querySelector('#latout-search-input');
const layout_search_btn = document.querySelector('#layout-search-btn');

layout_search_btn.addEventListener('click', function () {
    let searchUrl = "/Home/Search";
    let keyword = latout_search_input.value.trim();
    if (keyword) {
        searchUrl = searchUrl.concat(`?keyword=${keyword}`);
    }
    window.location.assign(searchUrl);
})