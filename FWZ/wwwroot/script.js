document.addEventListener('DOMContentLoaded', () => {
    const playerSelect = document.getElementById("playerNumber");
    for (let i = 1; i <= 10; i++) {
        const option = document.createElement("option");
        option.value = i;
        option.text = i;
        playerSelect.appendChild(option);
    }
});

async function fetchMatch() {
    const matchId = document.getElementById('matchId').value.trim();
    const playerNumber = document.getElementById('playerNumber').value;
    const loader = document.getElementById("loader");
    const blueTable = document.getElementById("blueTable");
    const redTable = document.getElementById("redTable");
    const blueTbody = blueTable.querySelector("tbody");
    const redTbody = redTable.querySelector("tbody");
    blueTbody.innerHTML = "";
    redTbody.innerHTML = "";
    blueTable.style.display = "none";
    redTable.style.display = "none";

    if (!matchId) { alert("Please enter a Match ID"); return; }
    loader.style.display = "block";

    let url = `/api/match/${matchId}`;

    try {
        const res = await fetch(url);
        loader.style.display = "none";

        if (!res.ok) { alert("Error fetching data"); return; }

        const data = await res.json();
        if (!data || data.length === 0) {
            alert("No data available for this match.");
            return;
        }

        function fillTable(team, tbody) {
            tbody.innerHTML = "";
            team.forEach(player => {
                const row = document.createElement("tr");
                row.innerHTML = `
                    <td>${player.summonerName || "Confidential"}</td>
                    <td>${player.championName}</td>
                    <td>${player.gold}</td>
                    <td>${player.kills}</td>
                    <td>${player.deaths}</td>
                    <td>${player.assists}</td>
                    <td>${player.cs}</td>
                    <td>${player.damageDealtToChampions}</td>
                    <td>${player.damageTaken}</td>
                `;
                tbody.appendChild(row);
            });
        }

        const container = document.querySelector(".container");

        if (!playerNumber) {
            const blueTeam = data.slice(0, 5);
            const redTeam = data.slice(5, 10);
            fillTable(blueTeam, blueTbody);
            fillTable(redTeam, redTbody);
            blueTable.style.display = "table";
            redTable.style.display = "table";
            container.style.maxWidth = "1100px";
        } else {
            const index = parseInt(playerNumber, 10) - 1;
            if (index >= 0 && index < data.length) {
                const player = data[index];
                fillTable([player], blueTbody);
                blueTable.style.display = "table";
                container.style.maxWidth = "1100px";
            }
        }

    } catch (err) {
        loader.style.display = "none";
        alert("Failed to fetch match data");
        console.error(err);
    }
}
