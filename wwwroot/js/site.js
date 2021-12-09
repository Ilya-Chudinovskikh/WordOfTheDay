const uri = 'api/words';
let words = [];

function getWords() {
    fetch(uri)
        .then(response => response.json())
        .then(data => _displayWords(data))
        .catch(error => console.error('Unable to show words.', error));
}

function getWordOfTheDay() {
    fetch(uri)
        .then(response => response.json())
        .then(data => _displayWordOfTheDay(data))
        .catch(error => console.error('Unable to show words.', error));
}

function addWord() {
    const addTextTextbox = document.getElementById('add-word');
    const addEmailTextbox = document.getElementById('add-email');
    const word = {
        email: addEmailTextbox.value.trim(),
        text: addTextTextbox.value.trim()
    };

    fetch(uri, {
        method: 'POST',
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(word)
    })
        .then(response => response.json())
        .then(() => {
            getWords();
            addEmailTextbox.value = '';
            addTextTextbox.value = '';
        })
        .catch(error => console.error('Unable to add your word.', error));
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

function _displayWordOfTheDay(data) {

    //import Enumerable from './linq.min.js'

    const tBody = document.getElementById('words');
    tBody.innerHTML = '';

    let wordOfTheDay = Enumerable.from(data).groupBy(w => w.Text).orderByDescending(w => w.count()).first();
    let number = Enumerable.from(data).count(w => w.Text == wordOfTheDay.Text);

    let tr = tBody.insertRow();

    let td1 = tr.insertCell(0);
    let wordNode = document.createTextNode(wordOfTheDay.text);
    td1.appendChild(wordNode);

    let td2 = tr.insertCell(1);
    let numberNode = document.createTextNode(number);
    td2.appendChild(numberNode);

    words = data;
}