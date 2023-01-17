import os
from flask import Flask, render_template, request, flash, redirect, url_for

app = Flask(__name__, template_folder="./")

@app.route('/')
def home():
    return redirect('/you-vs-ai')

@app.route('/you-vs-ai')
def playerVsAi():
    return render_template("index_playerVsAI.html")

@app.route('/ai-play')
def aiPlays():
    return render_template("index_AiPlays.html")

@app.route('/classic')
def playerPlays():
    return render_template("index_playerPlays.html")

if __name__ == "__main__":
    app.run(debug=True)