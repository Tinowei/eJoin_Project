class CustomImageUploadAdapter {
    constructor(loader) {
        this.loader = loader;
    }

    upload() {
        return this.loader.file
            .then(file => new Promise((resolve, reject) => {
                if (file.size > 4 * 1024 * 1024) {
                    reject('檔案大小超過4MB限制');
                } else {
                    const formData = new FormData();
                    formData.append('file', file);

                    fetch('/api/Upload/UploadImage', {
                        method: 'POST',
                        body: formData
                    })
                        .then(response => response.json())
                        .then(data => {
                            if (data && data.url) {
                                resolve({
                                    default: data.url
                                });
                            } else {
                                reject(data.error || '發生錯誤');
                            }
                        })
                        .catch(error => {
                            reject('上傳圖片時發生錯誤: ' + error.message);
                        });
                }
            }));

    }
    abort() {
        server.abortUpload();
    }
}
// CKEditor自定義上傳圖片
function CustomImageUploadAdapterPlugin(editor) {
    editor.plugins.get('FileRepository').createUploadAdapter = (loader) => {
        return new CustomImageUploadAdapter(loader);
    };
}

const { markRaw } = Vue;

const CreateEventApp = Vue.createApp({
    data() {
        return {
            themeJson: {},
            eventItem: {
                PictureUrl: "../../images/cardUploadImg.png",
                Name: "",
                StartTime: {
                    Date: "",
                    Time: "",
                },
                EndTime: {
                    Date: "",
                    Time: "",
                },
                City: 0,
                Address: "",
                AddressDetail: null,
                Latitude: "",
                Longitude: "",
                Summary: "",
                Intro: "",
                Tickets: [
                    //{
                    //    "Id": 1,
                    //    "Name": "General Admission",
                    //    "Amount": 11,
                    //    "Stock": 1,
                    //    "Price": 50,
                    //    "StartSellDate": "2024-04-02T17:43:00",
                    //    "EndSellDate": "2024-05-01T17:43:00",
                    //    "MaxPurchase": 5
                    //}
                ],
                themes: [
                    //{ id: 1, name: '藝文', image: 'new_arts.svg', selected: false },
                    // ...其他主題資訊
                ],
                selectedCount: 0,
            },
            currentStep: 1,
            isSubmitting: false,
            map: null,
            marker: null,
            mapApiKey: "AIzaSyBSOdeG7J9pLbc6nxGtMjNIM2h_RtitHvg",
        };
    },
    created() {
        this.themeJson = jsObj;

        if (this.themeJson) {
            this.eventItem.themes = this.themeJson.Theme.map(theme => ({
                ...theme,
                selected: false,
            }));
            this.eventItem.selectedCount = 0;
        }
    },
    mounted() {
        ClassicEditor
            .create(document.querySelector('#editor'), {
                extraPlugins: [CustomImageUploadAdapterPlugin],
                toolbar: {
                    items: [
                        'heading',
                        '|',
                        'bold',
                        'italic',
                        'link',
                        'bulletedList',
                        'numberedList',
                        '|',
                        'indent',
                        'outdent',
                        '|',
                        'imageUpload',
                        'blockQuote',
                        'insertTable',
                        'mediaEmbed',
                        'undo',
                        'redo',
                    ]
                },
                placeholder: '請輸入活動摘要...',
                language: 'zh',
                table: {
                    contentToolbar: [
                        'tableColumn',
                        'tableRow',
                        'mergeTableCells'
                    ]
                }
            })
            .then(editor => {
                const intro = '';
                editor.setData(intro);

                // 當編輯器內容變化時，更新Vue的數據
                editor.model.document.on('change:data', () => {
                    this.eventItem.Intro = editor.getData();
                });
            })
            .catch(error => {
                console.error(error.stack);
            });
        this.initMap();
    },
    methods: {
        triggerWindowResizeEvent() {
            setTimeout(() => {
                window.dispatchEvent(new Event('resize'));
            }, 1000);
        },
        toggleThemeSelection(theme) {
            if (theme.selected) {
                theme.selected = false;
                this.eventItem.selectedCount--;
            } else if (this.eventItem.selectedCount < 2) {
                theme.selected = true;
                this.eventItem.selectedCount++;
            }
        },
        isSelected(themeId) {
            const theme = this.themes.find(t => t.id === themeId);
            return theme && theme.selected ? 'selected' : '';
        },
        checkEndTime() {
            const startTime = new Date(this.eventItem.StartTime.Date + 'T' + this.eventItem.StartTime.Time);
            const endTime = new Date(this.eventItem.EndTime.Date + 'T' + this.eventItem.EndTime.Time);

            if (endTime < startTime) {
                this.showTimeNotCorrectAlert();
                this.eventItem.EndTime.Date = this.eventItem.StartTime.Date;
                this.eventItem.EndTime.Time = this.eventItem.StartTime.Time;

                return false;
            }

            return true;
        },
        checkEndTime1() {
            this.eventItem.Tickets.forEach(ticket => {
                const startTime = new Date(ticket.StartSellTime.Date + 'T' + ticket.StartSellTime.Time);
                const endTime = new Date(ticket.EndSellTime.Date + 'T' + ticket.EndSellTime.Time);

                if (endTime < startTime) {
                    this.showTimeNotCorrectAlert1();
                    ticket.EndSellTime.Date = ticket.StartSellTime.Date;
                    ticket.EndSellTime.Time = ticket.StartSellTime.Time;

                    return false;
                }

                return true;
            });
        },
        adjustToTimezone(date) {
            const time = new Date(date.getTime());
            time.setMilliseconds(time.getMilliseconds() + 8 * 3600000);
            return time.toISOString();
        },
        previewImage(event) {
            const file = event.target.files[0];
            if (file && file.size < 4 * 1024 * 1024) { // 檔案大小限制4MB
                const reader = new FileReader();
                reader.onload = (e) => {
                    this.eventItem.PictureUrl = e.target.result; // 更新預覽圖片
                };
                reader.readAsDataURL(file); // 讀取檔案並轉為DataURL
            } else {
                this.showFileSizeOver4mbAlert();
            }
        },
        previewMap() {
            // 定義 API 端點
            const apiEndpoint = `https://maps.googleapis.com/maps/api/geocode/json?address=台灣${this.eventItem.City}${this.eventItem.Address}&key=${this.mapApiKey}`;

            // 發送 fetch 請求
            fetch(apiEndpoint)
                .then(response => {
                    if (!response.ok) {
                        throw new Error(`HTTP error! status: ${response.status}`);
                    }
                    return response.json();
                })
                .then(data => {
                    // 檢查是否有結果
                    if (data.results.length === 0) {
                        throw new Error('No results found');
                    }
                    const location = data.results[0].geometry.location;
                    this.eventItem.Latitude = location.lat.toString();
                    this.eventItem.Longitude = location.lng.toString();

                    // 初始化地圖並設置marker
                    if (!this.map) {
                        this.initMap();
                    }
                    this.setMarker(this.eventItem.Latitude, this.eventItem.Longitude);
                    markRaw(this.map.flyTo([this.eventItem.Latitude, this.eventItem.Longitude], 18));
                })
                .catch(error => {
                    console.error('出現錯誤:', error);
                    this.showBasicErrorAlert();
                });
        },
        setMarker(lat, lng) {
            // 刪除原有的marker
            if (this.marker) {
                this.map.removeLayer(this.marker);
            }
            const newMarker = markRaw(L.marker([lat, lng]));

            markRaw(newMarker.bindPopup(`<b>${this.eventItem.City}${this.eventItem.Address}</b>`));

            markRaw(newMarker.addTo(this.map));

            this.marker = newMarker;
        },
        initMap() {
            this.map = markRaw(L.map('map').setView([25.0427344, 121.5325394], 15));

            markRaw(L.tileLayer('https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png', { minZoom: 8, maxZoom: 19 })
                .addTo(this.map));
        },
        submitImage() {
            const fileInput = document.getElementById('image-input-imgur');
            if (fileInput.files.length === 0) {
                return Promise.resolve(); // 如果沒有選擇檔案，直接解決 Promise
            } else {
                const file = fileInput.files[0];
                if (file.size > 4 * 1024 * 1024) {
                    this.showFileSizeOver4mbAlert(); // 提示檔案過大
                    return Promise.reject('檔案大小超過4MB限制');
                } else {
                    const formData = new FormData();
                    formData.append('file', file);
                    return fetch('/api/Upload/UploadImage', {
                        method: 'POST',
                        body: formData
                    }).then(response => {
                        if (!response.ok) {
                            throw new Error(`HTTP error! status: ${response.status}`);
                        }
                        return response.json();
                    }).then(data => {
                        this.eventItem.PictureUrl = data.url; // 將上傳後的圖片 URL 設定到 eventItem
                    }).catch(error => {
                        console.error('上傳圖片過程中出現錯誤:', error);
                        this.eventItem.PictureUrl = "../../images/cardUploadImg.png"; // 如果上傳失敗，重設圖片為預設值
                        throw error; // 將錯誤向外拋出，讓 overallSubmit 能捕獲
                    });
                }
            }
        },
        addTicket() {
            const timestamp = Date.now();
            const newTicket = {
                TempId: timestamp,
                Id: null,
                Name: '',
                Amount: 0,
                Stock: 0,
                Price: 0,
                MaxPurchase: null,
                StartSellDate: "",
                EndSellDate: "",
                StartSellTime: {
                    Date: '',
                    Time: '',
                },
                EndSellTime: {
                    Date: '',
                    Time: '',
                },
                isSettingsVisible: true,
                disabledMaxPurchase: false,
                isDeleted: 0,
            };
            this.eventItem.Tickets.push(newTicket);
        },
        async deleteTicket(ticketIdentifier) {
            // 使用 TempId 或 Id 來找到票券
            const ticketIndex = this.eventItem.Tickets.findIndex(ticket =>
                ticket.Id ? ticket.Id === ticketIdentifier : ticket.TempId === ticketIdentifier
            );

            // 如果找不到票券，直接返回
            if (ticketIndex === -1) {
                this.showBasicErrorAlert();
                return;
            }

            // 確認刪除
            const isConfirmed = await this.showAreYouSureAlert();
            if (isConfirmed) {
                this.eventItem.Tickets[ticketIndex].isDeleted = 1;
            }
        },
        toggleMaxPurchase(ticket) {
            if (ticket.disabledMaxPurchase) {
                ticket.MaxPurchase = ticket.MaxPurchase || 1;
            } else {
                ticket.MaxPurchase = null;
            }
        },
        submitSelection() {
            this.previewMap();

            // 收集被選取的項目 ID
            const selectedItems = this.eventItem.themes
                .filter(theme => theme.selected)
                .map(theme => theme.Id);

            const combineDateTime = (date, time) => {
                if (!date || !time) return null;
                return new Date(date + 'T' + time.padEnd(5, '0') + ':00');
            };

            const startDateTime = combineDateTime(this.eventItem.StartTime.Date, this.eventItem.StartTime.Time);
            const endDateTime = combineDateTime(this.eventItem.EndTime.Date, this.eventItem.EndTime.Time);

            //再檢查全部票券時間是否正確設定
            let hasIncorrectTime = false;

            for (const ticket of this.eventItem.Tickets) {
                const startTime = new Date(ticket.StartSellTime.Date + 'T' + ticket.StartSellTime.Time);
                const endTime = new Date(ticket.EndSellTime.Date + 'T' + ticket.EndSellTime.Time);

                if (endTime < startTime) {
                    ticket.EndSellTime.Date = ticket.StartSellTime.Date;
                    ticket.EndSellTime.Time = ticket.StartSellTime.Time;
                    this.showTimeNotCorrectAlert1();
                    hasIncorrectTime = true;
                    break;
                }
            }

            if (hasIncorrectTime) {
                return;
            }

            this.eventItem.Tickets.forEach(ticketData => {
                if (ticketData.isDeleted === 0) {
                    const startDateTime = combineDateTime(ticketData.StartSellTime.Date, ticketData.StartSellTime.Time);
                    const endDateTime = combineDateTime(ticketData.EndSellTime.Date, ticketData.EndSellTime.Time);

                    ticketData.StartSellDate = this.adjustToTimezone(startDateTime);
                    ticketData.EndSellDate = this.adjustToTimezone(endDateTime);
                }
                else if (ticketData.isDeleted === 1) {
                    ticketData.StartSellDate = this.adjustToTimezone(new Date());
                    ticketData.EndSellDate = this.adjustToTimezone(new Date());
                }
            });

            // 設定提交到 API 的資料，這裡結合了 eventId 和選擇的主題
            const requestData = {
                Name: this.eventItem.Name,
                PictureUrl: this.eventItem.PictureUrl,
                StartTime: startDateTime,
                EndTime: endDateTime,
                City: this.eventItem.City,
                Address: this.eventItem.Address,
                AddressDetail: this.eventItem.AddressDetail,
                Summary: this.eventItem.Summary,
                Intro: this.eventItem.Intro,
                Latitude: this.eventItem.Latitude,
                Longitude: this.eventItem.Longitude,
                SelectedThemes: selectedItems,
                TicketsData: this.eventItem.Tickets,
            };

            requestData.StartTime = this.adjustToTimezone(startDateTime);
            requestData.EndTime = this.adjustToTimezone(endDateTime);

            // 定義 API 端點
            const apiEndpoint = '/api/HostApi/CreateEvent';

            // 發送 fetch 請求
            fetch(apiEndpoint, {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json'
                },
                body: JSON.stringify(requestData)
            })
                .then(response => {
                    if (!response.ok) {
                        throw new Error(`HTTP error! status: ${response.status}`);
                    }
                    this.showBasicSuccessAlert();
                })
                .catch(error => {
                    console.error('更新時出現錯誤:', error);
                    this.showBasicErrorAlert();
                });
        },
        overallSubmit() {
            const isEndTimeValid = this.checkEndTime();
            if (!isEndTimeValid) {
                // 判斷活動結束時間是否設定正確
                return;
            }

            if (this.checkFormValidityForTicket()) {
                this.isSubmitting = true; // 開始提交流程，可以加入載入動畫

                // 首先調用 submitImage 上傳圖片
                this.submitImage().then(() => {
                    // 圖片上傳成功，繼續提交其他資料
                    return this.submitSelection();
                }).then(() => {
                    // 這裡可以處理表單資料提交成功後的反饋
                    this.isSubmitting = false; // 請求完成，解鎖按鈕
                }).catch(error => {
                    // 處理圖片上傳或表單資料提交過程中的錯誤
                    console.error('過程中出現錯誤:', error);
                    this.showImageUploadErrorAlert();
                    this.isSubmitting = false; // 錯誤發生，解鎖按鈕
                });
            }
        },
        checkFormValidityForBasic() {
            let isValid = true;
            const messages = [];

            if (!this.eventItem.Name) {
                messages.push('請填寫活動名稱。');
                isValid = false;
            }

            if (!this.eventItem.StartTime.Date || !this.eventItem.StartTime.Time) {
                messages.push('請填寫完整的活動開始時間。');
                isValid = false;
            }

            if (!this.eventItem.EndTime.Date || !this.eventItem.EndTime.Time) {
                messages.push('請填寫完整的活動結束時間。');
                isValid = false;
            }

            if (!this.eventItem.City) {
                messages.push('請選擇縣市。');
                isValid = false;
            }

            if (!this.eventItem.Address) {
                messages.push('請填寫地址。');
                isValid = false;
            }

            if (!isValid) {
                // 顯示所有的錯誤訊息
                this.showFormErrorAlert(messages.join('\n'));
                return false;
            }

            return true;
        },
        checkFormValidityForDesc() {
            let isValid = true;
            const messages = [];

            if (!this.eventItem.Summary) {
                messages.push('請填寫活動摘要。');
                isValid = false;
            }

            if (!this.eventItem.Intro) {
                messages.push('請填寫活動簡介。');
                isValid = false;
            }

            if (!isValid) {
                // 顯示所有的錯誤訊息
                this.showFormErrorAlert(messages.join('\n'));
                return false;
            }

            return true;
        },
        checkFormValidityForTicket() {
            let isValid = true;
            const messages = [];

            if (this.eventItem.Tickets.length > 0) {

                this.eventItem.Tickets.forEach((ticket) => {

                    if (ticket.isDeleted === 0) {

                        if (!ticket.Name) {
                            messages.push(`票券缺少名稱。`);
                            isValid = false;
                        }

                        if (!ticket.StartSellTime.Date || !ticket.StartSellTime.Time) {
                            messages.push(`票券缺少完整的販售開始時間。`);
                            isValid = false;
                        }

                        if (!ticket.EndSellTime.Date || !ticket.EndSellTime.Time) {
                            messages.push(`票券缺少完整的販售結束時間。`);
                            isValid = false;
                        }

                        if (ticket.Amount === undefined || ticket.Amount === null || isNaN(ticket.Amount) || ticket.Amount < 0) {
                            messages.push(`票券的販售數量必須是非負數字。`);
                            isValid = false;
                        }

                        if (ticket.Price === undefined || ticket.Price === null || isNaN(ticket.Price) || ticket.Price < 0) {
                            messages.push(`票券的販售價格必須是非負數字。`);
                            isValid = false;
                        }
                    }
                });
            }

            if (!isValid) {
                // 顯示所有的錯誤訊息
                this.showFormErrorAlert(messages.join('\n'));
                return false;
            }

            return true;
        },
        showThemeErrorAlert() {
            Swal.fire({
                icon: 'warning',
                title: '【更新時出現錯誤】',
                text: '請重試',
                showConfirmButton: true,
                confirmButtonText: '確認',
                heightAuto: false,
                customClass: {
                    title: 'S2_title_class',
                    content: 'S2_content_class',
                }
            });
        },
        showThemeSuccessAlert() {
            Swal.fire({
                icon: 'success',
                title: '主題更新成功!',
                showConfirmButton: true,
                confirmButtonText: '確認',
                heightAuto: false,
                customClass: {
                    title: 'S2_title_class',
                    content: 'S2_content_class',
                }
            });
        },
        showFileSizeOver4mbAlert() {
            Swal.fire({
                icon: 'warning',
                title: '【檔案大小超過4MB限制】',
                text: '請重新上傳圖片',
                showConfirmButton: true,
                confirmButtonText: '確認',
                heightAuto: false,
                customClass: {
                    title: 'S2_title_class',
                    content: 'S2_content_class',
                }
            });
        },
        showTimeNotCorrectAlert() {
            Swal.fire({
                icon: 'warning',
                title: '【活動結束時間不得早於開始時間】',
                text: '請重新設定時間',
                showConfirmButton: true,
                confirmButtonText: '確認',
                heightAuto: false,
                customClass: {
                    title: 'S2_title_class',
                    content: 'S2_content_class',
                }
            });
        },
        showTimeNotCorrectAlert1() {
            Swal.fire({
                icon: 'warning',
                title: '【票券結束時間不得早於開始時間】',
                text: '請重新設定時間',
                showConfirmButton: true,
                confirmButtonText: '確認',
                heightAuto: false,
                customClass: {
                    title: 'S2_title_class',
                    content: 'S2_content_class',
                }
            });
        },
        showImageUploadErrorAlert() {
            Swal.fire({
                icon: 'warning',
                title: '【上傳圖片時出現錯誤】',
                text: '請重試',
                showConfirmButton: true,
                confirmButtonText: '確認',
                heightAuto: false,
                customClass: {
                    title: 'S2_title_class',
                    content: 'S2_content_class',
                }
            });
        },
        showBasicSuccessAlert() {
            Swal.fire({
                icon: 'success',
                title: '活動建立成功!',
                text: '即將導向主辦中心，提醒您活動需手動上架',
                showConfirmButton: true,
                confirmButtonText: '確認',
                heightAuto: false,
                customClass: {
                    title: 'S2_title_class',
                    content: 'S2_content_class',
                }
            }).then(() => { window.location.assign("/Organize"); });
        },
        showBasicErrorAlert() {
            Swal.fire({
                icon: 'warning',
                title: '【更新時出現錯誤】',
                text: '請重試',
                showConfirmButton: true,
                confirmButtonText: '確認',
                heightAuto: false,
                customClass: {
                    title: 'S2_title_class',
                    content: 'S2_content_class',
                }
            });
        },
        showFormErrorAlert(message) {
            // 使用 message 變數並替換 \n 為 HTML 換行標籤 <br>
            const formattedMessage = message.replace(/\n/g, '<br>');

            Swal.fire({
                icon: 'warning',
                title: '【必填欄位沒有填寫】',
                html: formattedMessage,
                showConfirmButton: true,
                confirmButtonText: '確認',
                heightAuto: false,
                customClass: {
                    title: 'S2_title_class',
                    content: 'S2_content_class',
                }
            });
        },
        async showAreYouSureAlert() {
            return Swal.fire({
                icon: 'warning',
                title: '確認要刪除此票券?',
                showCancelButton: true,
                confirmButtonText: '確認',
                cancelButtonText: '取消',
                customClass: {
                    title: 'S2_title_class',
                    content: 'S2_content_class',
                }
            }).then((result) => {
                return result.isConfirmed;
            });
        },
        validateAndNextForBasic() {
            if (this.checkFormValidityForBasic()) {
                this.nextStep();
            }
        },
        validateAndNextForDesc() {
            if (this.checkFormValidityForDesc()) {
                this.nextStep();
            }
        },
        nextStep() {
            this.currentStep++;
            window.scrollTo({
                top: 0,
                behavior: 'smooth' // 使用平滑滾動
            });
            this.triggerWindowResizeEvent();
        },
        prevStep() {
            this.currentStep--;
            window.scrollTo({
                top: 0,
                behavior: 'smooth' // 使用平滑滾動
            });
            this.triggerWindowResizeEvent();
        }
    },
    computed: {
        isSaveBtnDisabled() {
            return this.eventItem.selectedCount === 0;
        },
        availableTickets() {
            return this.eventItem.Tickets.filter(ticket => ticket.isDeleted === 0);

        },
        todayDate() {
            const today = new Date();
            const year = today.getFullYear();
            const month = (today.getMonth() + 1).toString().padStart(2, '0'); // 將月份格式化為兩位數字
            const day = today.getDate().toString().padStart(2, '0'); // 將日期格式化為兩位數字
            return `${year}-${month}-${day}`; // 放回格式化為 YYYY-MM-DD 的日期字串
        }
    }
}).mount(".content");