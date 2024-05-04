const api = { getEvents: 'api/DashBoard' }
const apiCaller = { getDashBoard: () => httpGet(api.getEvents) };

const DashBoardApp = Vue.createApp({
    components: {
        EasyDataTable: window["vue3-easy-data-table"],
    },
    data() {
        return {
            loadingDashBoardData: false,
            dashBoardData: {
                "eventCount": 0,
                "orderCount": 0,
                "memberCount": 0,
                "totalPrice": 0,
                "amountGrowths": [],
                "memberGrowths": [],
                "themeCategories": [],
                "top5Events": []
            },
            top5EventsDataTableHeader: [
                { text: "活動名稱", value: "eventName" },
                { text: "主辦人", value: "memberName" },
                { text: "銷售量", value: "salesVolume" },
                { text: "銷售金額", value: "totalPrice" },
            ]
        }
    },
    methods: {
        //打api (Get)
        getDashBoardData() {
            this.loadingDashBoardData = true
            apiCaller.getDashBoard()
                .then(response => {
                    console.log("API Response:", response);
                    this.dashBoardData = response
                    this.createAmountGrowthAreaChart()
                    this.createMemberGrowthBarChart()
                    this.createThemeCategoryPieChart()
                })
                .catch(err => {
                    console.error(err);
                })
                .finally(() => {
                    this.loadingDashBoardData = false
                });
        },
        //收益成長Chart-Area
        createAmountGrowthAreaChart() {
            const myAreaChartCtx = document.getElementById("myAmountGrowthChart");
            let labels = []
            let data = []
            let amountGrowths = this.dashBoardData.amountGrowths
            let dataLength = amountGrowths.length
            if (dataLength > 0) {
                for (let i = 0; i < dataLength; i++) {
                    labels.push(amountGrowths[i].month)
                    data.push(amountGrowths[i].amount)
                }
            }

            const myAreaChart = new Chart(myAreaChartCtx, {
                type: 'line',
                data: {
                    labels: labels,
                    datasets: [{
                        label: "總金額成長",
                        lineTension: 0.3,
                        backgroundColor: "rgba(65,201,226,0.2)",
                        borderColor: "#41C9E2",
                        pointRadius: 5,
                        pointBackgroundColor: "#41C9E2",
                        pointBorderColor: "rgba(255,255,255,0.8)",
                        pointHoverRadius: 5,
                        pointHoverBackgroundColor: "#008DDA",
                        pointHitRadius: 50,
                        pointBorderWidth: 1,
                        data: data,
                    }],
                },
                options: {
                    scales: {
                        xAxes: [{
                            time: {
                                unit: 'date'
                            },
                            gridLines: {
                                display: false
                            },
                            ticks: {
                                maxTicksLimit: 12
                            }
                        }],
                        yAxes: [{
                            ticks: {
                                min: 0,
                                max: 50000,
                                maxTicksLimit: 20
                            },
                            gridLines: {
                                color: "rgba(0, 0, 0, .125)",
                            }
                        }],
                    },
                    legend: {
                        display: false
                    }
                }
            });
        },

        //會員成長Chart-Bar
        createMemberGrowthBarChart() {
            const myBarChartCtx = document.getElementById("myBarChart");
            let labels = []
            let data = []
            let memberGrowths = this.dashBoardData.memberGrowths
            let dataLength = memberGrowths.length
            if (dataLength > 0) {
                for (let i = 0; i < dataLength; i++) {
                    labels.push(memberGrowths[i].month)
                    data.push(memberGrowths[i].memberCount)
                }
            }

            const myBarChart = new Chart(myBarChartCtx, {
                type: 'bar',
                data: {
                    labels: labels,
                    datasets: [{
                        label: "會員成長",
                        backgroundColor: "#B7C9F2",
                        borderColor: "#B7C9F2",
                        data: data,
                    }],
                },
                options: {
                    scales: {
                        xAxes: [{
                            time: {
                                unit: 'month'
                            },
                            gridLines: {
                                display: false
                            },
                            ticks: {
                                maxTicksLimit: 12
                            }
                        }],
                        yAxes: [{
                            ticks: {
                                min: 0,
                                max: 100,
                                maxTicksLimit: 20
                            },
                            gridLines: {
                                display: true
                            }
                        }],
                    },
                    legend: {
                        display: false
                    }
                }
            });
        },

        //主題分布Chart-Pie
        createThemeCategoryPieChart() {
            const myPieChartCtx = document.getElementById("myPieChart");

            let labels = []
            let data = []
            let themeCategories = this.dashBoardData.themeCategories
            let dataLength = themeCategories.length
            if (dataLength > 0) {
                for (let i = 0; i < dataLength; i++) {
                    labels.push(themeCategories[i].name)
                    data.push(themeCategories[i].count)
                }
            }

            const myPieChart = new Chart(myPieChartCtx, {
                type: 'pie',
                data: {
                    labels: labels,
                    datasets: [{
                        data: data,
                        backgroundColor: ['#9195F6', '#B7C9F2', '#F9F07A', '#FB88B4', '#B5C0D0', '#CCD3CA', '#F5E8DD', '#EED3D9', '#CDFADB', '#F6FDC3', '#FFCF96', '#FF8080', '#5BBCFF', '#FFFAB7', '#FFD1E3', '#7EA1FF', '#FFF7FC', '#8B93FF', '#5755FE', '#EE99C2', '#F7B787', '#EE7214'],
                    }],
                },
            });
        },
    },

    mounted() {
        this.getDashBoardData();
    },
}).mount('#dashBoaard');