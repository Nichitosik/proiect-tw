document.addEventListener('DOMContentLoaded', (event) => {


    // Date pentru graficul donut
    const data = {
        labels: ['Mens', 'Womens', 'Kids'],
        datasets: [{
            label: 'Dataset',
            data: [28.4, 35.5, 26.5],
            backgroundColor: [
                '#3498db',
                '#2ecc71',
                '#1abc9c'
            ],
            hoverOffset: 3
        }]
    };

    // Configurare grafic
    const config = {
        type: 'doughnut',
        data: data,
        options: {
            responsive: true,
            plugins: {
                legend: {
                    position: 'top',
                },
                tooltip: {
                    callbacks: {
                        label: function (context) {
                            let label = context.label || '';
                            if (label) {
                                label += ': ';
                            }
                            if (context.parsed !== null) {
                                label += context.parsed + '%';
                            }
                            return label;
                        }
                    }
                }
            }
        }
    };

    // Inițializare grafic
    const ctx = document.getElementById('myChart').getContext('2d');
    const myChart = new Chart(ctx, config);
});
