var mongoose = require("mongoose");
var Schema = mongoose.Schema;

var Schema = new Schema({
    username:{
        type:String,
        require:true
    },
    password:{
        type:String,
        require:true
    },
    savedScore:{
        type:Number,
        require:true
    }
});

mongoose.model("accountSave", Schema);