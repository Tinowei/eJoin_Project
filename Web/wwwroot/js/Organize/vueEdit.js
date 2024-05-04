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

const themeSelectApp = Vue.createApp({
    data() {
        return {
            eventItem: {
            },
            themes: [
                //{ id: 1, name: '藝文', image: 'new_arts.svg', selected: false },
                // ...其他主題資訊
            ],
            selectedCount: 0,
            //LastEditTime: "",
            isSubmitting: false,
        };
    },
    created() {
        this.eventItem = jsObj;

        // 使用 eventItem.Theme 中的資訊來創建 themes 陣列
        if (this.eventItem && this.eventItem.Theme) {
            this.themes = this.eventItem.Theme.map(theme => ({
                ...theme,
                selected: this.eventItem.SelectedTheme.includes(theme.Id)
            }));
            this.selectedCount = this.eventItem.SelectedTheme.length;
        }
    },
    methods: {
        toggleThemeSelection(theme) {
            if (theme.selected) {
                theme.selected = false;
                this.selectedCount--;
            } else if (this.selectedCount < 2) {
                theme.selected = true;
                this.selectedCount++;
            }
        },
        isSelected(themeId) {
            const theme = this.themes.find(t => t.id === themeId);
            return theme && theme.selected ? 'selected' : '';
        },
        submitSelection() {
            // 收集被選取的項目 ID
            const selectedItems = this.themes
                .filter(theme => theme.selected)
                .map(theme => theme.Id);

            const eventId = this.eventItem.Id;

            //this.LastEditTime = this.adjustToTimezone3(new Date());

            // 設定提交到 API 的資料，這裡結合了 eventId 和選擇的主題
            const requestData = {
                eventId: eventId,
                selectedThemes: selectedItems,
            };

            // 定義 API 端點
            const apiEndpoint = '/api/OrganizeEdit/EditTheme';

            this.isSubmitting = true;

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
                    this.showThemeSuccessAlert();
                })
                .catch(error => {
                    console.error('更新時出現錯誤:', error);
                    this.showThemeErrorAlert();
                })
                .finally(() => {
                    this.isSubmitting = false;  // 請求完成，解鎖按鈕
                });
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
        adjustToTimezone3(date) {
            const time = new Date(date.getTime());
            time.setMilliseconds(time.getMilliseconds() + 8 * 3600000);
            return time.toISOString();
        },
    },
    computed: {
        isSaveBtnDisabled() {
            return this.selectedCount === 0 || this.isSubmitting === true;
        }
    }
}).mount(".select-theme-content");

const { markRaw } = Vue;

const basicInfoApp = Vue.createApp({
    data() {
        return {
            eventItem: {
            },
            basicInfo: {
                EventId: 0,
                PictureUrl: "",
                Name: "",
                StartTime: {
                    Date: "",
                    Time: "",
                },
                EndTime: {
                    Date: "",
                    Time: "",
                },
                City: "",
                Address: "",
                AddressDetail: "",
                Latitude: "",
                Longitude: "",
                //LastEditTime: "",
            },
            availableCities: [
                '台北市', '新北市', '基隆市', '桃園市', '新竹市', '新竹縣', '苗栗縣', '台中市', '彰化縣', '南投縣', '雲林縣', '嘉義市', '嘉義縣', '台南市', '高雄市', '屏東縣', '宜蘭縣', '花蓮縣', '台東縣', '澎湖縣', '金門縣', '連江縣'
            ],
            isSubmitting: false,
            map: null,
            marker: null,
            mapApiKey: "AIzaSyBSOdeG7J9pLbc6nxGtMjNIM2h_RtitHvg",
        };
    },
    created() {
        this.eventItem = jsObj;
        if (this.eventItem) {
            this.basicInfo.EventId = this.eventItem.Id;

            this.basicInfo.PictureUrl = this.eventItem.PictureUrl || "";
            this.basicInfo.Name = this.eventItem.Name || "";

            const start = new Date(this.eventItem.StartTime);
            const end = new Date(this.eventItem.EndTime);

            this.basicInfo.StartTime.Date = start.toLocaleDateString('en-CA') || "";
            this.basicInfo.StartTime.Time = start.toTimeString().slice(0, 5) || "";

            this.basicInfo.EndTime.Date = end.toLocaleDateString('en-CA') || "";
            this.basicInfo.EndTime.Time = end.toTimeString().slice(0, 5) || "";


            this.basicInfo.City = this.eventItem.City || 0;
            this.basicInfo.Address = this.eventItem.Address || "";
            this.basicInfo.AddressDetail = this.eventItem.AddressDetail || "";

            this.basicInfo.Latitude = this.eventItem.Latitude || "";
            this.basicInfo.Longitude = this.eventItem.Longitude || "";

        }
    },
    computed: {
        isLocked() {
            //return this.eventItem.Status === 2 || this.eventItem.Status === 3;
            let isValid = false;
            this.eventItem.Tickets.forEach(ticket => {

                if (ticket.Amount != ticket.Stock) {
                    isValid = true;
                }
            });

            if (isValid) {
                return true;
            }
            return false;
        },
        todayDate() {
            const today = new Date();
            const year = today.getFullYear();
            const month = (today.getMonth() + 1).toString().padStart(2, '0'); // 將月份格式化為兩位數字
            const day = today.getDate().toString().padStart(2, '0'); // 將日期格式化為兩位數字
            return `${year}-${month}-${day}`; // 放回格式化為 YYYY-MM-DD 的日期字串
        }
    },
    methods: {
        validateCitySelection() {
            if (!this.availableCities.includes(this.basicInfo.City)) {
                this.basicInfo.City = "0";
            }
        },
        checkEndTime() {
            const startTime = new Date(this.basicInfo.StartTime.Date + 'T' + this.basicInfo.StartTime.Time);
            const endTime = new Date(this.basicInfo.EndTime.Date + 'T' + this.basicInfo.EndTime.Time);

            if (endTime < startTime) {
                this.showTimeNotCorrectAlert();
                this.basicInfo.EndTime.Date = this.basicInfo.StartTime.Date;
                this.basicInfo.EndTime.Time = this.basicInfo.StartTime.Time;

                return false;
            }

            return true;
        },
        adjustToTimezone(date) {
            const time = new Date(date.getTime());
            time.setMilliseconds(time.getMilliseconds() + 8 * 3600000);
            return time.toISOString();
        },
        previewImage(event) {
            const file = event.target.files[0];
            const reader = new FileReader();
            reader.onload = (e) => {
                this.basicInfo.PictureUrl = e.target.result; // 更新預覽圖片
            };
            reader.readAsDataURL(file); // 讀取檔案並轉為DataURL
        },
        previewMap() {
            // 定義 API 端點
            const apiEndpoint = `https://maps.googleapis.com/maps/api/geocode/json?address=台灣${this.basicInfo.City}${this.basicInfo.Address}&key=${this.mapApiKey}`;

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
                    this.basicInfo.Latitude = location.lat.toString();
                    this.basicInfo.Longitude = location.lng.toString();

                    // 初始化地圖並設置marker
                    if (!this.map) {
                        this.initMap();
                    }
                    this.setMarker(this.basicInfo.Latitude, this.basicInfo.Longitude);
                    markRaw(this.map.flyTo([this.basicInfo.Latitude, this.basicInfo.Longitude], 18));
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
            
            markRaw(newMarker.bindPopup(`<b>${this.basicInfo.City}${this.basicInfo.Address}</b>`));
            
            markRaw(newMarker.addTo(this.map));
            
            this.marker = newMarker;
        },
        initMap() {
            this.map = markRaw(L.map('map').setView([this.basicInfo.Latitude, this.basicInfo.Longitude], 18));

            markRaw(L.tileLayer('https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png', { minZoom: 8, maxZoom: 19 })
                .addTo(this.map));
        },
        submitImage() {
            const fileInput = document.getElementById('image-input-imgur');
            // 檢查是否有檔案被選取
            if (fileInput.files.length === 0 || this.basicInfo.PictureUrl === this.eventItem.PictureUrl) {
                return Promise.resolve(); // 如果沒有選擇檔案，直接解決 Promise
            } else {
                const file = fileInput.files[0];
                if (file.size > 4 * 1024 * 1024) {
                    // 如果檔案大於4MB，提示用戶並不執行後續操作
                    this.showFileSizeOver4mbAlert();
                    return Promise.reject('檔案大小超過4MB限制');
                } else {
                    const formData = new FormData();
                    formData.append('file', file);

                    return fetch('/api/Upload/UploadImage', {
                        method: 'POST',
                        body: formData
                    })
                        .then(response => {
                            if (!response.ok) {
                                throw new Error(`HTTP error! status: ${response.status}`);
                            }
                            return response.json();
                        })
                        .then(data => {
                            this.basicInfo.PictureUrl = data.url;
                        })
                        .catch(error => {
                            console.error('上傳圖片過程中出現錯誤:', error);
                            this.showImageUploadErrorAlert();
                        });
                }
            }
        },
        submitSelection1() {
            this.previewMap();

            const combineDateTime = (date, time) => {
                if (!date || !time) return null;
                return new Date(date + 'T' + time.padEnd(5, '0') + ':00');
            };

            const startDateTime = combineDateTime(this.basicInfo.StartTime.Date, this.basicInfo.StartTime.Time);
            const endDateTime = combineDateTime(this.basicInfo.EndTime.Date, this.basicInfo.EndTime.Time);

            const basicInfoData = {
                ...this.basicInfo,
                StartTime: startDateTime,
                EndTime: endDateTime
            };

            basicInfoData.StartTime = this.adjustToTimezone(startDateTime);
            basicInfoData.EndTime = this.adjustToTimezone(endDateTime);
            //basicInfoData.LastEditTime = this.adjustToTimezone(new Date());

            // 設定提交到 API 的資料
            const requestData = basicInfoData;

            // 定義 API 端點
            const apiEndpoint = '/api/OrganizeEdit/EditBasicInfo';

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
            if (!isEndTimeValid) { // 判斷時間是否設定正確
                return;
            }

            if (this.checkFormValidityForBasic()) {
                this.isSubmitting = true;

                this.submitImage().then(() => {
                    // 圖片上傳成功，繼續提交其他資料
                    return this.submitSelection1();
                }).catch(error => {
                    console.error('過程中出現錯誤:', error);
                    this.showImageUploadErrorAlert();
                }).finally(() => {
                    this.isSubmitting = false;  // 請求完成，解鎖按鈕
                });
            }
        },
        checkFormValidityForBasic() {
            let isValid = true;
            const messages = [];

            if (!this.basicInfo.Name) {
                messages.push('請填寫活動名稱。');
                isValid = false;
            }

            if (!this.basicInfo.StartTime.Date || !this.basicInfo.StartTime.Time) {
                messages.push('請填寫完整的活動開始時間。');
                isValid = false;
            }

            if (!this.basicInfo.EndTime.Date || !this.basicInfo.EndTime.Time) {
                messages.push('請填寫完整的活動結束時間。');
                isValid = false;
            }

            if (!this.basicInfo.City) {
                messages.push('請選擇縣市。');
                isValid = false;
            }

            if (!this.basicInfo.Address) {
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
                title: '【結束時間不得早於開始時間】',
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
                title: '活動更新成功!',
                /*text: '張票券已核銷!',*/
                showConfirmButton: true,
                confirmButtonText: '確認',
                heightAuto: false,
                customClass: {
                    title: 'S2_title_class',
                    content: 'S2_content_class',
                }
            });
        },
    },
    mounted() {
        this.validateCitySelection();

        // 初始化地圖
        this.initMap();

        // 在預設的位置上設置marker
        this.setMarker(this.basicInfo.Latitude, this.basicInfo.Longitude);
    }
}).mount(".basic-info-content");

const descriptionApp = Vue.createApp({
    data() {
        return {
            eventItem: {
            },
            descInfo: {
                EventId: 0,
                Summary: "",
                Intro: "",
                //LastEditTime: "",
            },
            isSubmitting: false,
        };
    },
    created() {
        this.eventItem = jsObj;
        if (this.eventItem) {
            this.descInfo.EventId = this.eventItem.Id;

            this.descInfo.Summary = this.eventItem.Summary || "";
            this.descInfo.Intro = this.eventItem.Intro || "";
        }
    },
    methods: {
        adjustToTimezone1(date) {
            const time = new Date(date.getTime());
            time.setMilliseconds(time.getMilliseconds() + 8 * 3600000);
            return time.toISOString();
        },
        submitSelection2() {
            //this.descInfo.LastEditTime = this.adjustToTimezone1(new Date());

            // 設定提交到 API 的資料
            const requestData = this.descInfo;

            // 定義 API 端點
            const apiEndpoint = '/api/OrganizeEdit/EditDescInfo';

            if (this.checkFormValidityForDesc()) {
                this.isSubmitting = true;

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
                        this.showBasicSuccessAlert1();
                    })
                    .catch(error => {
                        console.error('更新時出現錯誤:', error);
                        this.showBasicErrorAlert1();
                    })
                    .finally(() => {
                        this.isSubmitting = false;  // 請求完成，解鎖按鈕
                    });
            }
        },
        checkFormValidityForDesc() {
            let isValid = true;
            const messages = [];

            if (!this.descInfo.Summary) {
                messages.push('請填寫活動摘要。');
                isValid = false;
            }

            if (!this.descInfo.Intro) {
                messages.push('請填寫活動簡介。');
                isValid = false;
            }

            if (!isValid) {
                // 顯示所有的錯誤訊息
                this.showFormErrorAlert1(messages.join('\n'));
                return false;
            }

            return true;
        },
        showFormErrorAlert1(message) {
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
        showFileSizeOver4mbAlert1() {
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
        showBasicErrorAlert1() {
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
        showImageUploadErrorAlert1() {
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
        showBasicSuccessAlert1() {
            Swal.fire({
                icon: 'success',
                title: '活動更新成功!',
                showConfirmButton: true,
                confirmButtonText: '確認',
                heightAuto: false,
                customClass: {
                    title: 'S2_title_class',
                    content: 'S2_content_class',
                }
            });
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
                editor.setData(this.descInfo.Intro);

                // 當編輯器內容變化時，更新Vue的數據
                editor.model.document.on('change:data', () => {
                    this.descInfo.Intro = editor.getData();
                });
            })
            .catch(error => {
                console.error(error.stack);
            });
    }
}).mount(".event-description-content");

const ticketApp = Vue.createApp({
    data() {
        return {
            eventItem: {
            },
            tickets: {
                EventId: 0,
                ticketsData: [
                    //{
                    //    "Id": 1,
                    //    "Name": "General Admission",
                    //    "Amount": 100,
                    //    "Price": 50,
                    //    "StartSellDate": "2024-04-02T17:43:29.9",
                    //    "EndSellDate": "2024-05-01T17:43:29.9",
                    //    "MaxPurchase": 5
                    //}
                ],
                //LastEditTime: "",
            },
            isSubmitting: false,
        };
    },
    created() {
        this.eventItem = jsObj;

        if (this.eventItem) {
            this.tickets.EventId = this.eventItem.Id;

            this.tickets.ticketsData = this.eventItem.Tickets.map(tk => {
                const startSell = new Date(tk.StartSellDate);
                const endSell = new Date(tk.EndSellDate);

                return {
                    ...tk,
                    StartSellTime: {
                        Date: startSell.toLocaleDateString('en-CA') || "",
                        Time: startSell.toTimeString().slice(0, 5) || "",
                    },
                    EndSellTime: {
                        Date: endSell.toLocaleDateString('en-CA') || "",
                        Time: endSell.toTimeString().slice(0, 5) || "",
                    },
                    isSettingsVisible: false,
                    disabledMaxPurchase: false,
                    soldTickets: 0,
                    isDeleted: 0,
                };
            });

            this.tickets.ticketsData.forEach(ticket => {
                ticket.disabledMaxPurchase = ticket.MaxPurchase != null;
                ticket.soldTickets = ticket.Amount - ticket.Stock;
            });
        }
    },
    methods: {
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
            this.tickets.ticketsData.push(newTicket);
        },
        async deleteTicket(ticketIdentifier) {
            // 使用 TempId 或 Id 來找到票券
            const ticketIndex = this.tickets.ticketsData.findIndex(ticket =>
                ticket.Id ? ticket.Id === ticketIdentifier : ticket.TempId === ticketIdentifier
            );

            // 如果找不到票券，直接返回
            if (ticketIndex === -1) {
                this.showBasicErrorAlert2();
                return;
            }

            // 確認刪除

            if (this.tickets.ticketsData[ticketIndex].Id != null) {
                const amount = this.tickets.ticketsData[ticketIndex].Amount;
                const stock = this.tickets.ticketsData[ticketIndex].Stock;

                if (amount !== stock) {
                    this.showTicketDeleteErrorAlert();
                    return;
                }
            }


            const isConfirmed = await this.showAreYouSureAlert();
            if (isConfirmed) {
                this.tickets.ticketsData[ticketIndex].isDeleted = 1;
            }
        },
        toggleMaxPurchase(ticket) {
            if (ticket.disabledMaxPurchase) {
                ticket.MaxPurchase = ticket.MaxPurchase || 1;
            } else {
                ticket.MaxPurchase = null;
            }
        },
        isTicketLocked(ticket) {
            return ticket.Amount != ticket.Stock && ticket.Id != null;
        },
        adjustToTimezone2(date) {
            const time = new Date(date.getTime());
            time.setMilliseconds(time.getMilliseconds() + 8 * 3600000);
            return time.toISOString();
        },
        checkTicketAmounts() {
            for (const ticket of this.tickets.ticketsData) {
                if (ticket.Amount < ticket.soldTickets) {
                    return false;
                }
            }
            return true;
        },
        //fetchEditViewModel() {
        //    const apiEndpoint = '/api/OrganizeEdit/GetEventData';
        //    const requestData = this.tickets.EventId;

        //    fetch(apiEndpoint, {
        //        method: 'POST',
        //        headers: {
        //            'Content-Type': 'application/json'
        //        },
        //        body: JSON.stringify(requestData)
        //    })
        //        .then(response => {
        //            if (!response.ok) {
        //                throw new Error('更新活動出現錯誤');
        //            }
        //            return response.json();
        //        })
        //        .then(data => {
        //            this.eventItem = {};
        //            this.eventItem = data;
        //        })
        //        .catch(error => {
        //            console.error('更新活動出現錯誤:', error);
        //        });
        //},
        checkEndTime1() {
            this.tickets.ticketsData.forEach(ticket => {
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
        submitSelection3() {

            // 定義 API 端點
            const apiEndpoint = '/api/OrganizeEdit/EditTicket';

            if (this.checkFormValidityForTicket()) {
                //再檢查全部票券時間是否正確設定
                let hasIncorrectTime = false;

                for (const ticket of this.tickets.ticketsData) {
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

                //檢查票券設定數量是否小於售出量
                if (!this.checkTicketAmounts()) {
                    this.showTicketAmountErrorAlert();
                    return;
                }

                //組合時間
                const combineDateTime = (date, time) => {
                    if (!date || !time) return null;
                    return new Date(date + 'T' + time.padEnd(5, '0') + ':00');
                };

                this.tickets.ticketsData.forEach(ticketData => {
                    if (ticketData.isDeleted == 0) {
                        const startDateTime = combineDateTime(ticketData.StartSellTime.Date, ticketData.StartSellTime.Time);
                        const endDateTime = combineDateTime(ticketData.EndSellTime.Date, ticketData.EndSellTime.Time);

                        ticketData.StartSellDate = this.adjustToTimezone2(startDateTime);
                        ticketData.EndSellDate = this.adjustToTimezone2(endDateTime);
                    }
                    else if (ticketData.isDeleted == 1) {
                        ticketData.StartSellDate = this.adjustToTimezone2(new Date());
                        ticketData.EndSellDate = this.adjustToTimezone2(new Date());
                    }
                });

                //this.tickets.LastEditTime = this.adjustToTimezone2(new Date());
                // 設定提交到 API 的資料
                const requestData = this.tickets;

                this.isSubmitting = true;

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
                        this.showBasicSuccessAlert2();
                    })
                    .catch(error => {
                        console.error('更新時出現錯誤:', error);
                        this.showBasicErrorAlert2();
                    })
                    .finally(() => {
                        this.isSubmitting = false;
                        //this.fetchEditViewModel();
                        //this.initialize();
                    });
            }
        },
        //initialize() {
        //    this.tickets.ticketsData = [];

        //    this.tickets.ticketsData = this.eventItem.Tickets.map(tk => {
        //        const startSell = new Date(tk.StartSellDate);
        //        const endSell = new Date(tk.EndSellDate);

        //        return {
        //            ...tk,
        //            StartSellTime: {
        //                Date: startSell.toLocaleDateString('en-CA') || "",
        //                Time: startSell.toTimeString().slice(0, 5) || "",
        //            },
        //            EndSellTime: {
        //                Date: endSell.toLocaleDateString('en-CA') || "",
        //                Time: endSell.toTimeString().slice(0, 5) || "",
        //            },
        //            isSettingsVisible: false,
        //            disabledMaxPurchase: false,
        //            soldTickets: 0,
        //        };
        //    });

        //    this.tickets.ticketsData.forEach(ticket => {
        //        ticket.disabledMaxPurchase = ticket.MaxPurchase != null;
        //        ticket.soldTickets = ticket.Amount - ticket.Stock;
        //    });
        //},
        checkFormValidityForTicket() {
            let isValid = true;
            const messages = [];

            if (this.tickets.ticketsData.length > 0) {

                this.tickets.ticketsData.forEach((ticket) => {

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
                this.showFormErrorAlert2(messages.join('\n'));
                return false;
            }

            return true;
        },
        showFormErrorAlert2(message) {
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
        showBasicErrorAlert2() {
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
        showTicketAmountErrorAlert() {
            Swal.fire({
                icon: 'warning',
                title: '【票券的數量不得小於已售出的數量】',
                text: '請重新設定',
                showConfirmButton: true,
                confirmButtonText: '確認',
                heightAuto: false,
                customClass: {
                    title: 'S2_title_class',
                    content: 'S2_content_class',
                }
            });
        },
        showTicketDeleteErrorAlert() {
            Swal.fire({
                icon: 'warning',
                title: '【此票券已有人購買無法刪除】',
                showConfirmButton: true,
                confirmButtonText: '確認',
                heightAuto: false,
                customClass: {
                    title: 'S2_title_class',
                    content: 'S2_content_class',
                }
            });
        },
        showBasicSuccessAlert2() {
            Swal.fire({
                icon: 'success',
                title: '活動更新成功!',
                showConfirmButton: true,
                confirmButtonText: '確認',
                heightAuto: false,
                customClass: {
                    title: 'S2_title_class',
                    content: 'S2_content_class',
                }
            }).then(() => { window.location.reload(); });
        },
        showTimeNotCorrectAlert1() {
            Swal.fire({
                icon: 'warning',
                title: '【結束時間不得早於開始時間】',
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
    },
    computed: {
        availableTickets() {
            return this.tickets.ticketsData.filter(ticket => ticket.isDeleted === 0);
        },
        todayDate() {
            const today = new Date();
            const year = today.getFullYear();
            const month = (today.getMonth() + 1).toString().padStart(2, '0'); // 將月份格式化為兩位數字
            const day = today.getDate().toString().padStart(2, '0'); // 將日期格式化為兩位數字
            return `${year}-${month}-${day}`; // 放回格式化為 YYYY-MM-DD 的日期字串
        }
    },
}).mount(".ticket-setting-content");

////地圖API
//const {  markRaw } = Vue; //解構出createApp  and markRaw
//var url = '';
//var headers = new Headers({
//    'Content-Type': 'application/json',
//});
////start to use createApp object
//createApp({
//    data() {
//        return {
//            apiKey: '', //iI6e2Owzpmf7cpgGGwQvZ4prklgIcnDG
//            init: {
//                method: 'GET',
//                headers: new Headers({
//                    'Content-Type': 'application/json',
//                }),
//                mode: 'cors',
//                cache: 'default'
//            },
//            myAddress: '台北市大安區忠孝東路三段96號',
//            map: null,
//            marker: markRaw(new tt.Marker({ width: '50', height: '56', scale: 1 })),
//            popup: markRaw(new tt.Popup({ offset: 50 })),

//        }
//    },
//    mounted() {
//        this.initMap();

//    },
//    methods: {
//        initMap() { //初始化地圖
//            this.map = markRaw(tt.map({
//                key: this.apiKey,
//                container: 'map',
//                dragPan: !isMobileOrTablet(),
//                center: [121.53625, 25.04166],
//                zoom: 18,
//            }));
//            this.map.addControl(new tt.FullscreenControl()); //可全螢幕
//            this.map.addControl(new tt.NavigationControl());
//            this.marker.setLngLat([121.53625, 25.04166]).addTo(this.map);
//            this.popup.setText(this.myAddress);
//            this.marker.setPopup(this.popup);
//        },
//        getJsonAddress(myAddress) {
//            url = 'https://api.tomtom.com/search/2/geocode/' + encodeURIComponent(this.myAddress) + '.json?key=' + this.apiKey;

//            fetch(url, this.init)
//                .then(response => response.json())
//                .then(data => {
//                    if (data.results.length > 0) {
//                        let position = data.results[0].position;
//                        this.flyToNewLocation(position.lat, position.lon);
//                    }
//                })

//                .catch(error => console.error('發生錯誤：', error), console.log(url));
//        },
//        flyToNewLocation(lat, lon) {
//            markRaw(this.map.flyTo({
//                center: [lon, lat],
//                zoom: 18,
//                speed: 1,
//                curve: 1,
//            }));
//            if (this.marker) { //當地圖上已經有一個標記時，刪除標記
//                this.marker.remove();
//            }
//            markRaw(this.marker.setLngLat([lon, lat]).addTo(this.map));
//            markRaw(this.popup.setText(this.myAddress));
//            markRaw(this.marker.setPopup(this.popup.setText(this.myAddress)));
//        },
//    }
//}).mount('.map-container');