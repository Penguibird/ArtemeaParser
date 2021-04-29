const fs = require('fs');
const parse = require('csv-parse');
const { v4: uuidv4, parse: uuidParse } = require('uuid');
const MUUID = require('uuid-mongodb');
const { get } = require('http');


const uuid = () => {
    let id = uuidv4();
    return id
}


const path = './book1.csv' //skills start at 5

fs.readFile(path, (err, data) => {
    // let data = data.toString();
    parse(data, {}, (error, data) => {
        let [skills, positions] = parseData(data)
        console.log(`Parsed ${skills.length} skills.`);
        console.log(`Parsed ${positions.length} positions.`);

        fs.writeFileSync('./skills.json', JSON.stringify(skills));
        fs.writeFileSync('./positions.json', JSON.stringify(positions));
    })
})

const isString = (level) => level != '' && level && level != 'not applicable' && level != 'not applicable ' && level != 'NA';

function* getHorPos() {
    let n = 0;

    while (true) {
        yield n += 1000;
        console.log(n);
    }
}


const getTier = (i) => {
    return -i * 200
}

const parseData = (data) => {
    let positions = {}
    let gp = getHorPos();

    let skills = data.map((skill, sIndex) => {
        let s;
        let positionName = isString(s = skill[1]) ? s : skill[0]//`${skill[1]} TS `
        positionName = positionName[positionName.length - 1] === ' ' ? positionName.slice(0, positionName.length - 1) : positionName;
        let levels = [];
        skill.slice(4).forEach(level => {
            if (isString(level) && !levels.includes(level)) {
                levels.push(level)
            }
        })
        // filter((level, i, arr) => isString(level) && !levels.includes(level))
        let id = uuid();
        let minLevel;
        for (let i = 5; i < 13; i++) {
            if (isString(skill[i])) {
                minLevel = i;
                break;
            }
        }
        var getPos;
        var gpCurr;
        for (let i = 1; i <= 5; i++) {
            getPos = (j) => {
                return i == 1 ? gpCurr = gp.next().value : gpCurr
            }
            if (i == 1 ) {
                gpCurr = gp.next().value;
            }
            if (isString(skill[4 + i])) {
                if (!positions[`${positionName} TS ${i}`]) {
                    // console.log(positions, `${positionName} TS ${i}`, )
                    Object.defineProperty(positions, `${positionName} TS ${i}`, {
                        value: {
                            toId: uuid(),
                            Tier: getTier(i),
                            HorizontalPosition: gpCurr,
                            Name: `${positionName} TS ${i}`.replace(/^\s*[\r\n]/gm, ""),
                            RequiredSkills: [{
                                SkillToId: id,
                                Level: levels.indexOf(skill[4 + i]) + 1 //4 + i - minLevel + 1
                            }]
                        },
                        enumerable: true
                    });
                    if (i == 1) console.log(gpCurr)
                } else {
                    positions[`${positionName} TS ${i}`].RequiredSkills.push({
                        SkillToId: id,
                        Level: levels.indexOf(skill[4 + i]) + 1 //4 + i - minLevel + 1
                    });
                }

            }
        }
        for (let i = 0; i < 3; i++) {
            if (isString(skill[10 + i])) {
                if (!positions[`${positionName} IA ${i + 3}`]) {
                    Object.defineProperty(positions, `${positionName} IA ${i + 3}`, {
                        value: {
                            toId: uuid(),
                            Name: `${positionName} IA ${i + 3}`.replace(/^\s*[\r\n]/gm, ""),
                            Tier: getTier(i + 5),
                            HorizontalPosition: gpCurr,
                            RequiredSkills: [{
                                SkillToId: id,
                                Level: levels.indexOf(skill[10 + i]) + 1 //10 + i - minLevel + 1
                            }]
                        },
                        enumerable: true
                    })
                } else {
                    positions[`${positionName} IA ${i + 3}`].RequiredSkills.push({
                        SkillToId: id,
                        Level: levels.indexOf(skill[10 + i]) + 1 // 10 + i - minLevel + 1
                    })
                }
            }
        }


        return {
            toId: id,
            Name: skill[4].replace(/^\s*[\r\n]/gm, ""),
            Description: `Product: ${skill[0]}`,
            Levels: levels.map(l => {
                l.replace(/^\s*[\r\n]/gm, "");
                return l
            }),
            Category: 0,
        }
    });

    const removeTail = (string) => {
        let ret = (string + '').split(' ').slice(0, (string + '').split(' ').length - 3).join(' ');
        // console.log(string)
        return ret
    }

    positions = Object.values(positions).map((position, i, array) => {
        if(!position.HorizontalPosition) console.log(position.Name, i)
        if (i == array.length - 1) return position
        if (removeTail(position.Name) == removeTail(array[i + 1].Name)) {
            array[i + 1].childToId = position.toId;

            if (position.HorizontalPosition != array[i + 1].HorizontalPosition) {
                array[i + 1].HorizontalPosition = position.HorizontalPosition;
            }
        }
        return position
    })
    return [skills, positions]
}

const example = {
    "_id": {
        "$binary": {
            "base64": "w6HCqcKnbUbCvV1GwqNQcwnCt8KbwoIi",
            "subType": "00"
        }
    },
    "Name": "C#",
    "Description": "Knowledge of C#",
    "MaxLevel": 5,
    "Levels": [
        "Test",
        "Test"
    ],
    "Category": 0
}