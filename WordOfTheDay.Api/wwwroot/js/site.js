const uri = 'api/words';
let words = [];

function getWords() {
    fetch(uri)
        .then(response => response.json())
        .then(data => _displayWords(data))
        .catch(error => console.error('Unable to show words.', error));
}

async function addWord() {
    const addTextTextbox = document.getElementById('add-word');
    const addEmailTextbox = document.getElementById('add-email');
    const word = {
        email: addEmailTextbox.value.trim(),
        text: addTextTextbox.value.trim()
    };

    const response = await fetch(uri, {
        method: 'POST',
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(word)
    });
    if (!(response.ok === true)) {
        const errorData = await response.json();
        console.log("errors", errorData);
        if (errorData) {
            if (errorData.errors) {
                if (errorData.errors["Email"]) {
                    addError(errorData.errors["Email"]);
                }
                if (errorData.errors["Text"]) {
                    addError(errorData.errors["Text"]);
                }
            }
            if (errorData["Email"]) {
                addError(errorData["Email"]);
            }
            if (errorData["Text"]) {
                addError(errorData["Text"]);
            }
        }
        document.getElementById("errors").style.display = "block";
    }
}

function addError(errors) {

    errors.forEach(error => {
        const p = document.createElement("p");
        p.append(error);
        document.getElementById("errors").append(p);
    })
}

function _displayWords(data) {

    const tBody = document.getElementById('words');
    tBody.innerHTML = '';

    data.forEach(word => {

        let tr = tBody.insertRow();

        let td1 = tr.insertCell(0);
        let emailNode = document.createTextNode(word.email);
        td1.appendChild(emailNode);

        let td2 = tr.insertCell(1);
        let textNode = document.createTextNode(word.text);
        td2.appendChild(textNode);

    });

    words = data;
}
