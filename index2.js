const fs = require('fs');

fs.readFile('./positions.json', (error, data) => {
    let data2 = JSON.parse(data).map(pos => {

        pos.RequiredSkills.forEach(skill => {
            delete skill._id;
            delete skill.toId;
            return skill
        })
        return pos
    })
    fs.writeFile('./positions2.json', JSON.stringify(data2), () => { })
})