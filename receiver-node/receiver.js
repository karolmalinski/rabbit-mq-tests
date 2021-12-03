var amqp = require('amqplib/callback_api');
var protobuf = require("protobufjs");

amqp.connect('amqp://localhost', function (error0, connection) {
    if (error0) {
        throw error0;
    }
    connection.createChannel(function (error1, channel) {
        if (error1) {
            throw error1;
        }

        var queue = 'person';

        channel.assertQueue(queue, {
            durable: false
        });

        console.log(" [*] Waiting for messages in %s. To exit press CTRL+C", queue);

        channel.consume(queue, function (msg) {

            const personModel = protobuf.loadSync("../proto-files/Person.proto");
            const personMessage = personModel.lookupType("Person");

            // Decode an Uint8Array (browser) or Buffer (node) to a message
            const message = personMessage.decode(msg.content);
            const str = JSON.stringify(message);

            console.log(" [x] Received %s", str);
        }, {
            noAck: true
        });
    });
});
