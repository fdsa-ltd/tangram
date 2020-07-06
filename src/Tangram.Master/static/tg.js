var tg = {};
tg.ebus = [];
tg.nbus = [];
tg.title = getQueryString("_title");
if (tg.title) {
    tg.find = function (name) {
        if (!name) {
            name = tg.title;
        }
        return new form(name);
    }
    tg.open = function (url, features) {
        var name = tg.ws.sendTo({
            from: (new Date()).valueOf(),
            to: name,
            type: "open",
            data: [url, features]
        });
        var result = new form(name);
        return result;
    }
    tg.run = function (filename, args) {
        var result = new form(name);
        tg.ws.sendTo({ from: name, type: "run", data: [filename, args] });
        return result;
    }
    tg.store = new store();
    tg.ws = new WS();
} else {
    tg.find = function (name) {
        return window.external.find(name);
    }
    tg.open = function (url, name, type, features, parent) {
        return window.external.open(url, name, type, features, parent);
    }
    tg.run = function (filename, args) {
        return window.external.run(filename, args);
    }
    tg.store = window.external.store;
}

function form(name) {
    this.name = name;
    this.show = function (parent) {
        tg.ws.sendTo({ from: tg.title, to: this.name, type: "invoke", data: ["show", parent] });
        return this;
    }
    this.hide = function () {
        tg.ws.sendTo({ from: tg.title, to: this.name, type: "invoke", data: ["hide"] });
        return this;
    }
    this.close = function () {
        tg.ws.sendTo({ from: tg.title, to: this.name, type: "invoke", data: ["close"] });

        return this;
    }
    this.refresh = function (url) {
        tg.ws.sendTo({ from: tg.title, to: this.name, type: "invoke", data: ["refresh", url] });
        return this;
    }
    this.site = function (left, top) {
        tg.ws.sendTo({ from: tg.title, to: this.name, type: "invoke", data: ["site", left, top] });
        return this;
    }
    this.size = function (width, height) {
        tg.ws.sendTo({ from: tg.title, to: this.name, type: "invoke", data: ["size", width, height] });
        return this;
    }
    this.mode = function (status) {
        tg.ws.sendTo({ from: tg.title, to: this.name, type: "invoke", data: ["mode", status] });

        return this;
    }
    this.exec = function (script, asyn) {
        tg.ws.sendTo({ from: tg.title, to: this.name, type: "invoke", data: ["exec", script, asyn] });
        return this;
    }
}

function store() {
    this.name = tg.title;
    this.get = function (key, callback) {
        var result;
        $.ajax({
            type: "post",
            url: "http://localhost:5555/post/get",
            contentType: 'application/json',
            data: JSON.stringify({ type: "get", from: this.name, data: [key] }),
            success: function (data) {
                var result = JSON.parse(data);
                callback(result);
            }
        });
    }
    this.set = function (key, value, timeout) {
        var data = JSON.stringify({ from: this.name, type: "set", data: [key, value, timeout] });
        tg.ws.send(data);
    }
}


function mq() {
    this.state = window.external.state();
    this.on = function (eventName, callback) {
        tg.ebus[eventName] = callback;
        this.state.on(eventName, callback);
    };
    this.send = function (formName, eventName, data, ack) {
        var message = JSON.stringify(data);
        if (typeof (ack) == "function") {
            ack(this.state.send(formName, eventName, message));
        } else {
            this.state.send(formName, eventName, message);
        }
    };
}

function io(url, options) {
    this.url = url;
    this.options = JSON.stringify(options);
    this.socket = window.external.socket(this.url, this.options);
    this.connected = function () {
        return this.socket == null;
    }
    this.disconnected = function () {
        return this.socket != null;
    }
    this.on = function (eventName, callback) {
        this.socket.on(eventName);
        window.nbus[eventName] = callback;
    };
    this.open = this.connect = function () {
        this.socket.open();
    };
    this.close = this.disconnect = function () {
        this.socket.close();
    }
    this.send = this.emit = function (to, data, ack) {
        if (typeof (ack) == "function") {
            ack(this.socket.send(to, data));
        } else {
            this.socket.send(to, data);
        }
    };
}

function WS() {
    this.ws = new WebSocket("ws://localhost:5555/tg?title=" + tg.title);
    this.ws.onopen = function () {
        console.log("Openened connection to websocket");
    };
    this.ws.onclose = function () {
        console.log("Close connection to websocket");
        tg.ws = new WS();
    }
    this.ws.onmessage = function (e) {
        var message = JSON.parse(e.data);
        if (message.type == "find" || message.type == "open") {
            if (this.results[message.from] == null) {
                this.results = message.data[0];
            } else {
                this.results[message.from](message.data[0]);
            }
            return;
        }
        if (message.type == "exec") {
            eval(message.data[0]);
        }
    }
    this.results = [];
    this.sendTo = function (message, callback) {
        if (message.type == "find" || message.type == "open") {
            if (!callback) {
                this.results[message.from] = null;
            } else {
                this.results[message.from] = callback;
            }
            this.ws.send(JSON.stringify(message));
            if (!callback) {
                return this.results[message.from];
            }
        }
        this.ws.send(JSON.stringify(message));
    }
    return this;
}

function getQueryString(name) {
    var reg = new RegExp("(^|&)" + name + "=([^&]*)(&|$)", "i");
    var reg_rewrite = new RegExp("(^|/)" + name + "/([^/]*)(/|$)", "i");
    var r = window.location.search.substr(1).match(reg);
    var q = window.location.pathname.substr(1).match(reg_rewrite);
    if (r != null) {
        return unescape(r[2]);
    } else if (q != null) {
        return unescape(q[2]);
    } else {
        return null;
    }
}
// function* main() {
// 	for (var i = 0; i < 10; i++) {
// 		yield 10;
// 	}
// 	yield 5;
// 	console.log('I just slept 5 milliseconds!');
// }
// function resume(ms, generator) {
// 	setTimeout(function () {
// 		var nextSleep = generator.next().value;
// 		resume(nextSleep, generator);
// 	}, ms);
// }
// function sleep() {
// 	var generator = main();
// 	var firstSleep = generator.next().value;
// 	resume(firstSleep, generator);
// }