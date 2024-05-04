const chartElement = document.getElementById('myChart');
    const data = {
        labels: [
            '報名人數',
            '剩餘名額',
        ],
        datasets: [{
            label: '人數',
            data: [100, 30],
        }]
    };
    new Chart(chartElement, {
        type: 'doughnut',
        data: data,
    });