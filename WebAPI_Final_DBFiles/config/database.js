if(process.env.NODE_ENV === "production"){
    module.exports = {mongoURI:"mongodb+srv://JakeFontaine:4f2tQ4ps450bi9Nx@cluster0.ctj3bfb.mongodb.net/?retryWrites=true&w=majority"}
}
else{
    module.exports = {mongoURI:"mongodb://127.0.0.1:27017/accountSaves"}
}