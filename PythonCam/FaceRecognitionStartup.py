import os

while True:
    os.system(f"sudo /usr/bin/python3 -u /home/pi/Desktop/FaceRecognition.py -ip 192.168.2.10 -port 8000 -api http://192.168.2.10:5000/api | sudo tee /var/log/face.log")
