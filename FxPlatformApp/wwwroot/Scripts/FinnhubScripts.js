//create a WebSocket to perform duplex (back and forth) communication with server
const token = document.getElementById("FinnhubToken").value;
const socket = new WebSocket(`wss://ws.finnhub.io?token=${token}`);
var stockSymbol = document.getElementById("StockSymbol").value; //get symbol from hidden input

//Subscribe to symbol on opened connection
socket.addEventListener("open", event => {
    socket.send(JSON.stringify({ "type": "subscribe", "symbol": stockSymbol }))
});

//listen for messages
socket.addEventListener("message", event => {
    if (event.data.type === "error") {
        $(".price").text(event.data.msg);
        return;
    }

    //data received from server
    //console.log('Message from server ', event.data);

    /* Sample response:
    {"data":[{"p":220.89,"s":"MSFT","t":1575526691134,"v":100}],"type":"trade"}
    type: message type
    data: [ list of trades ]
    s: symbol of the company
    p: Last price
    t: UNIX milliseconds timestamp
    v: volume (number of orders)
    c: trade conditions (if any)
    */

    var eventData = JSON.parse(event.data);
    if (eventData) {
        if (eventData.data) {
            //get the updated price
            var updatedPrice = JSON.parse(event.data).data[0].p;
            var timeStamp = JSON.parse(event.data).data[0].t;
            console.log(updatedPrice, timeStamp);
            console.log(new Date(timeStamp).toLocaleDateString());

            //UI realtime update
            $(".price").text(updatedPrice.toFixed(2));
            $("#price").val(updatedPrice.toFixed(2));

        }
    }
});

var unsubscribe = symbol => {
    //sends unsubscribe signal to disconnect from server
    socket.send(JSON.stringify({'type': 'unsubscribe', 'symbol': symbol}))
};

//unsubscribe from socket when window closes
window.onunload = () => {
    unsubscribe(stockSymbol);
};