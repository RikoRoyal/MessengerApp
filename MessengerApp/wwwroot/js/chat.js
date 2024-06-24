"use strict";

var connection = new signalR.HubConnectionBuilder().withUrl("/chathub").configureLogging(signalR.LogLevel.Information).build();

connection.on("ReceiveMessage", function (fromUser, message) {
    var msg = message.replace(/&/g, "&amp;").replace(/</g, "&lt;").replace(/>/g, "&gt;");
    var encodedMsg = fromUser + ": " + msg;
    var li = document.createElement("li");
    li.classList.add("list-group-item");
    li.textContent = encodedMsg;
    document.getElementById("messagesList").appendChild(li);
});

connection.start().catch(function (err) {
    return console.error(err.toString());
});

$('#sendButton').on('click', function () {
    var chatId = $('#chatList .active').data('chat-id');
    var message = $('#messageInput').val();
    var sender = $('#userInput').val();
    if (chatId && message) {
        fetch(`/api/chat/${chatId}/send`, {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify({
                fromUser: sender,
                toUser: $('#recipientInput').val(),
                message: message
            })
        })
            .then(response => {
                if (!response.ok) {
                    throw new Error('Network response was not ok');
                }
                return response.json();
            })
            .then(data => {
                console.log('Message sent:', data);
                $('#messageInput').val('');
                loadMessages(chatId);
            })
            .catch(error => {
                console.error('Error sending message:', error);
            });
    } else {
        alert("No active chat selected.");
    }
});

document.getElementById("chatList").addEventListener("click", function (event) {
    if (event.target && event.target.matches("li.list-group-item")) {
        var chatId = event.target.getAttribute("data-chat-id");
        var activeElement = document.querySelector("#chatList .active");
        if (activeElement) {
            activeElement.classList.remove("active");
        }
        event.target.classList.add("active");
        loadMessages(chatId);
    }
});

document.getElementById("searchButton").addEventListener("click", function (event) {
    var username = document.getElementById("searchUser").value;
    console.log(`Searching for user: ${username}`);
    fetch(`/Chat/SearchUser?username=${username}`)
        .then(response => {
            if (!response.ok) {
                console.error('Network response was not ok');
                throw new Error('Network response was not ok');
            }
            return response.json();
        })
        .then(data => {
            console.log('Response data:', data);
            if (data.success) {
                displaySearchResults([data.user]);
                document.getElementById("recipientInput").value = data.user.username;
            } else {
                alert(data.message);
            }
        })
        .catch(error => {
            console.error('Error:', error);
            alert('Error: ' + error.message);
        });
    event.preventDefault();
});

function displaySearchResults(users) {
    var searchResults = document.getElementById("searchResults");
    searchResults.innerHTML = "";

    users.forEach(user => {
        var li = document.createElement("li");
        li.classList.add("list-group-item");
        li.textContent = user.username;
        li.addEventListener("click", function () {
            openChat(user.id, user.username);
        });
        searchResults.appendChild(li);
    });
}

function openChat(chatId, username) {
    var chatHeader = document.createElement("li");
    chatHeader.classList.add("list-group-item", "active");
    chatHeader.textContent = `Chat with ${username}`;
    document.getElementById("messagesList").innerHTML = "";
    document.getElementById("messagesList").appendChild(chatHeader);
    document.getElementById("userInput").value = username;
    document.getElementById("recipientInput").value = username;

    var activeChatElement = document.querySelector("#chatList .list-group-item.active");
    if (activeChatElement) {
        activeChatElement.classList.remove("active");
    }
    var newActiveChatElement = document.querySelector(`#chatList .list-group-item[data-chat-id='${chatId}']`);
    if (newActiveChatElement) {
        newActiveChatElement.classList.add("active");
    }

    loadMessages(chatId);
}

function populateChatList() {
    console.log('populateChatList called');
    fetch('/Chat/GetChats')
        .then(response => response.json())
        .then(data => {
            console.log('Chats data:', data);
            var chatList = document.getElementById("chatList");
            chatList.innerHTML = "";
            data.forEach(chat => {
                var li = document.createElement("li");
                li.classList.add("list-group-item");
                li.textContent = chat.name;
                li.setAttribute("data-chat-id", chat.id);
                li.addEventListener("click", function () {
                    openChat(chat.id, chat.name);
                });
                chatList.appendChild(li);
            });
        })
        .catch(error => {
            console.error('Error loading chats:', error);
        });
}

function updateChatList(user) {
    console.log('updateChatList called');
    fetch('/Chat/GetChats')
        .then(response => response.json())
        .then(data => {
            console.log('Chats data:', data);
            var chatList = document.getElementById("chatList");
            chatList.innerHTML = "";
            data.forEach(chat => {
                var li = document.createElement("li");
                li.classList.add("list-group-item");
                li.textContent = chat.name;
                li.setAttribute("data-chat-id", chat.id);
                li.addEventListener("click", function () {
                    openChat(chat.id, chat.name);
                });
                chatList.appendChild(li);
            });
        })
        .catch(error => {
            console.error('Error updating chat list:', error);
        });
}

populateChatList();

function loadMessages(chatId) {
    fetch(`/api/chat/${chatId}/messages`)
        .then(response => {
            if (!response.ok) {
                throw new Error('Network response was not ok');
            }
            return response.json();
        })
        .then(data => {
            var messagesList = document.getElementById("messagesList");
            messagesList.innerHTML = "";
            data.forEach(message => {
                var li = document.createElement("li");
                li.classList.add("list-group-item");
                li.innerHTML = `<strong>${message.username}:</strong> ${message.text} <span class="text-muted">(${new Date(message.timestamp).toLocaleString()})</span>`;
                messagesList.appendChild(li);
            });
        })
        .catch(error => {
            console.error('Error loading messages:', error);
        });
}
