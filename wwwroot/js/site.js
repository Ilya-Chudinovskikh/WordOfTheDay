const uri = 'api/words'
let words = [];

function getWords() {
    fetch(uri)
        .then(response => response.json())
        .then(data => _displayWords(data))
        .catch(error => console.error('Unable to show words.', error));
}

function addWord() {
    const addTextTextbox = document.getElementById('add-word');
    const addEmailTextbox = document.getElementById('add-email');
    const word = {
        email: addEmailTextbox.value.trim(),//добавить проверку на емайл и слово макс 50 символов
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