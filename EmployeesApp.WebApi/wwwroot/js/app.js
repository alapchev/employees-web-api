const filePicker = document.getElementById('filepicker');
const submitBtn = document.getElementById('submitbtn');
const jsonCheck = document.getElementById('show-json');
const jsonArea = document.getElementById('json');
const messages = document.querySelector('.messages');
const results = document.getElementById('results-tbody');
const resultsContainer = document.querySelector('.results');
const uri = 'api/employees';

submitBtn.addEventListener('click', submitData);
jsonCheck.addEventListener('change', showJson);

async function submitData() {
    clearPage();

    const file = filepicker.files[0];
    if (!file) {
        setErrorMessage('No file selected');
        return;
    }

    const text = await file.text();
    const lines = text.split(/\r?\n/).filter(x => x.length !== 0);
    const data = { records: [] };

    let i = 0;
    if (lines.length !== 0) {
        const regex = /^\D/;
        if (regex.test(lines[0].trimStart())) {
            i = 1;
        }
    }

    for (; i < lines.length; i++) {
        const tokens = lines[i].split(',').map(t => t.trim());
        data.records.push({
            employeeId: tokens[0],
            projectId: tokens[1],
            dateFrom: tokens[2],
            dateTo: tokens[3]
        });
    }

    if (data.records.length === 0) {
        setErrorMessage('No data found to submit');
        return;
    }

    fetch(uri, {
        method: 'POST',
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(data)
    })
        .then(response => response.json())
        .then(obj => showResults(obj))
        .catch(err => console.error('problem sending data', err));
}

function showResults(data) {
    resultsContainer.style.display = 'block';
    jsonArea.value = JSON.stringify(data, null, 2);

    if (data.length === 0) {
        const row = results.insertRow();
        const cell = row.insertCell();
        cell.colSpan = 3;
        cell.textContent = 'No employees who have worked together found';
        return;
    }

    for (const pair of data) {
        const row = results.insertRow();
        const cell1 = row.insertCell();
        cell1.textContent = `${pair.employee1Id}, ${pair.employee2Id}`;
        const cell2 = row.insertCell();
        cell2.textContent = pair.commonProjectsIds.join(', ');
        const cell3 = row.insertCell();
        cell3.textContent = pair.daysWorkedTogether;
    }
}

function showJson() {
    if (this.checked) {
        jsonArea.style.display = 'block';
    } else {
        jsonArea.style.display = 'none';
    }
}

function setErrorMessage(errorText) {
    resultsContainer.style.display = 'none';
    messages.firstChild.textContent = errorText;
    messages.style.display = 'block';
}

function clearPage() {
    messages.style.display = 'none';
    jsonArea.value = '';
    while (results.firstChild) {
        results.removeChild(results.firstChild);
    }
}
