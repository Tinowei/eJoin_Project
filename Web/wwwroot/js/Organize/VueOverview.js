const { createApp } = Vue;

createApp({
    data() {
        return {
            eventItem: {},
            checkInChartData: {
                labels: ['未報到', '已報到'],
                datasets: [{
                    label: '人數',
                    data: [], // 動態更新
                }]
            },
            charts: [],
        };
    },
    created() {
        try {
            this.eventItem = jsObj;
            this.checkInChartData.datasets[0].data = [this.eventItem.CheckInStatus.NotCheckedInTickets, this.eventItem.CheckInStatus.CheckedInTickets];
        } catch (error) {
            console.error(error);
        }
    },
    methods: {
        initChart() {
            // 初始化所有的圖表
            this.eventItem.Tickets.forEach((ticket, index) => {
                const chartData = {
                    labels: ['已售出', '剩餘'],
                    datasets: [{
                        label: '張數', // 票種名稱
                        data: [ticket.Amount - ticket.Stock, ticket.Stock], // 售出數量和剩餘數量
                    }]
                };

                // 確保對應的canvas元素存在
                const chartElement = document.getElementById(`myChart-${index}`);
                // 初始化或更新Charts實例
                if (this.charts[index]) {
                    this.charts[index].data = chartData;
                    this.charts[index].update();
                } else {
                    this.charts[index] = new Chart(chartElement, {
                        type: 'doughnut',
                        data: chartData,
                        options: {
                                responsive: true,
                                plugins: {
                                    legend: {
                                        position: 'top',
                                    },
                                    title: {
                                        display: true,
                                        text: ticket.Name,
                                    }
                                }
                            }
                    });
                }
            });
        },
        initCheckInChart() {
            // 透過 refs 獲取 DOM 元素
            const chartElement = this.$refs.checkInChart;
            new Chart(chartElement, {
                type: 'doughnut',
                data: this.checkInChartData,
            });
        },
        async submitStatus() {
            const eventId = this.eventItem.Id;
            const requestData = {
                eventId: eventId,
            };

            const apiEndpoint = '/api/OrganizeEdit/EditStatus';

            const isConfirmed = await this.showAreYouSureAlert();
            if (isConfirmed) {
                try {
                    const response = await fetch(apiEndpoint, {
                        method: 'POST',
                        headers: {
                            'Content-Type': 'application/json'
                        },
                        body: JSON.stringify(requestData)
                    });
                    if (!response.ok) {
                        throw new Error(`HTTP error! status: ${response.status}`);
                    }
                    await this.showBasicSuccessAlert();
                } catch (error) {
                    console.error(error);
                    await this.showBasicErrorAlert();
                }
            }
        },
        async showAreYouSureAlert() {
            return Swal.fire({
                icon: 'warning',
                title: '確認要執行此動作?',
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
        directToEdit() {
            if (this.eventItem.StatusInt === 3 || this.eventItem.StatusInt === 4) {
                // 阻止導航行為
                event.preventDefault();
                this.showEditErrorAlert();
            } else {
                // 正常導航到編輯頁面
                window.location.href = `/Organize/Edit/${this.eventItem.Id}`;
            }
        },
        showEditErrorAlert() {
            Swal.fire({
                icon: 'warning',
                title: '【無法編輯】',
                text: '這個活動的狀態不允許進行編輯',
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
        showBasicSuccessAlert() {
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
    },
    mounted() {
        this.initChart();
        this.initCheckInChart();
    },
    computed: {
        buttonLabel() {
            switch (this.eventItem.StatusInt) {
                case 1: return '上架';
                case 2: return '下架';
                case 3: return '已結束';
                case 4: return '已下架';
                default: return '';
            }
        }
    }
}).mount(".overview-content");