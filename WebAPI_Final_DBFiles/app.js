var express = require("express");
var app = express();
var path = require("path");
var bodyParser = require("body-parser");
var mongoose = require("mongoose");
var port = process.env.port||3000;
var db = require("./config/database");
const { Console } = require("console");

app.use(bodyParser.json());
app.use(bodyParser.urlencoded({extended:true}));
app.use(express.json());

mongoose.connect(db.mongoURI,{
    useNewURLParser:true
}).then(function(){
    console.log("Connected to MongoDB Database");
}).catch(function(err){
    console.log(err);
});

require("./modules/UnitySchema");
var Accounts = mongoose.model("accountSave");

// DONT TOUCH
/****************************************************************************/

app.get("/", function(req, res){
    res.redirect("AccountList.html");
})

app.get("/getAccounts", function(req, res){
    Accounts.find({}).sort({"username":1}).then(function(accounts)
    {
        res.json({accounts});
    });
})

app.post("/deleteAccount", function(req, res){
    console.log(`Account Deleted ${req.body.username}`);
    Accounts.findByIdAndDelete(req.body.username).exec();
    res.redirect('AccountList.html');
})

app.post("/updateAccount", function(req, res){
    Accounts.findByIdAndUpdate(req.body.id, {username:req.body.username, password:req.body.password,
        savedScore:req.body.score}, function()
    {
        res.redirect("/AccountList.html");
    });    
})

// DATABASE
/***************/
// UNITY

app.post("/login", function(req, res){
    Accounts.find({"username":req.body.username}).then(function(accountsaves)
    {
        res.send({accountsaves});
    })
});

app.post("/newAccount", function(req, res){
    var newData = {
        "username" : req.body.username,
        "password" : req.body.password,
        "savedScore" : 0
    }
    console.log(newData);

    new Accounts(newData).save(function(err, room)
    {
        //room._id to return id and store it in singleton
        res.send(room._id);
    });
});

app.post("/saveAccount", function(req, res){
    console.log(req.body);
    Accounts.findByIdAndUpdate(req.body._id, {savedScore:req.body.savedScore}, function(){
        console.log("Updated Account " + req.body._id);
    })
})

/****************************************************************************/
// DONT TOUCH
app.use(express.static(__dirname+"/pages"));
app.listen(port, function(){
    console.log(`Running on port ${port}`);
})