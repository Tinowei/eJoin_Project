var tickets;
var ticketsJson;
var eventId = eventId;
window.onload =function() {
    sendTicketsToServer();
}


//從sessionStorage提取購買票券資訊
function TicketDtetailFromSession() {
    ticketsJson = sessionStorage.getItem("tickets");
    if (ticketsJson) {
        tickets = JSON.parse(ticketsJson);
        tickets = tickets.filter(ticket => ticket.count > 0);
        console.log(tickets);
        return tickets;
    }
}


function sendTicketsToServer() {
    tickets = TicketDtetailFromSession();
    console.log("事件觸發");
    console.log(ticketsJson); //字串有東西
    if (tickets.length>0) {
        let t = { Tickets: tickets };
        console.log(t);
        //console.log(tickets); //陣列包物件有東西
        $.ajax({
            url: `/Register/Complete?EventId=${eventId}`,
            type: 'POST',
            contentType: 'application/json; charset=utf-8',
            data: JSON.stringify(t),
            dataType: 'json',
            success: function (response) {
                console.log('打中:', response);
            },
            error: function (error) {
                console.error('Error sending data:', error);
            }
        });
    }
}