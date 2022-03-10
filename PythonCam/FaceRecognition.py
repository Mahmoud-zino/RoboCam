#!/usr/bin/python3
import numpy as np
from picamera import PiCamera
from picamera.array import PiRGBArray
import argparse
import cv2
import requests
import time
import socket


# ArgParser parse all the arguments defined by the user
# Argument list:
# mandatory => ip, port
# optional => width, height, framerate, jpeg_quality
class ArgParser:
    def __init__(self):
        self.argParser = argparse.ArgumentParser(allow_abbrev=False)

        # Mandatory
        self.argParser.add_argument("-ip", action="store", required=True)
        self.argParser.add_argument("-port", action="store", required=True)
        self.argParser.add_argument("-api", action="store", required=True)

        # Optional
        self.argParser.add_argument("-width", action="store", type=int, default=640, required=False)
        self.argParser.add_argument("-height", action="store", type=int, default=480, required=False)
        self.argParser.add_argument("-framerate", action="store", type=int, default=30, required=False)
        self.argParser.add_argument("-jpeg_quality", action="store", type=int, default=80, required=False)

    def extract_args(self):
        try:
            return self.argParser.parse_args()
        except Exception as e:
            print(f"Can't Extract Arguments:\n{e}")


class Camera:
    def __init__(self, args):
        try:
            self.cam = PiCamera()
            self.cam.resolution = (args.width, args.height)
            self.cam.framerate = args.framerate
            self.cam.saturation = 20
            self.cam.rotation = 90
            self.cam.hflip = True
            self.raw_capture = self.init_raw_capture(args)
            # warm up camera
            self.cam.start_preview()
            time.sleep(2)
        except Exception as e:
            # Mayor error
            print(f'Cant initialize Cam {e}')

    def init_raw_capture(self, args):
        try:
            return PiRGBArray(self.cam, size=(args.width, args.height))
        except Exception as e:
            print('Cant initialize RawCapture')


class ApiManager:
    def __init__(self, url):
        self.url = url

    def post_camera(self, args):
        try:
            data = {'width': args.width, 'height': args.height}
            request = requests.post(url=self.url + '/Camera', json=data, verify=False, timeout=1)
            if request.status_code != 200:
                print(f'status code: {str(request.status_code)}')
                time.sleep(1)
                print('post camera failed, trying it again!')
                self.post_camera(args)
        except:
            print("Post camera request timed out, trying it again!")
            self.post_camera(args)

    def post_face(self, x, y, width, height):
        try:
            data = {'xPoint': x, 'yPoint': y, 'width': width, 'height': height}
            request = requests.post(url=f'{self.url}/face', json=data, verify=False, timeout=1)
            if request.status_code != 200:
                print(f'status code: {str(request.status_code)}')
                time.sleep(1)
                print('post Face failed, trying it again!')
                self.post_face(x, y, width, height)
        except:
            print("Post face request timed out, trying it again!")
            self.post_face(x, y, width, height)

    def post_face_count(self, count):
        try:
            data = {'faceCount': int(count)}
            request = requests.post(url=f'{self.url}/faceCount', json=data, verify=False, timeout=1)
            if request.status_code != 200:
                print(f'status code: {str(request.status_code)}')
                time.sleep(1)
                print('post Face Count failed, trying it again!')
                self.post_face_count(count)
        except:
            print("Post face count request timed out, trying it again!")
            self.post_face_count(count)


class StreamManager:
    def __init__(self, args):
        self.encode_param = [int(cv2.IMWRITE_JPEG_QUALITY), args.jpeg_quality]
        # initialize UDP_Socket
        self.client = socket.socket(socket.AF_INET, socket.SOCK_DGRAM)
        self.ip = args.ip
        self.port = args.port

    def send_image(self, img):
        buffer = cv2.imencode('.JPEG', img, self.encode_param)[1].tobytes()
        self.client.sendto(buffer, (str(self.ip), int(self.port)))


def main():
    last_face_count = -1
    args = ArgParser().extract_args()
    pi_cam = Camera(args)

    apiManager = ApiManager(args.api)
    streamManager = StreamManager(args)

    raw_capture = pi_cam.raw_capture
    face_cascade = cv2.CascadeClassifier(f"{cv2.data.haarcascades}haarcascade_frontalface_default.xml")

    # Try is here so when ctrl + c is clicked the program stop
    try:
        # Main Routine
        while True:
            apiManager.post_camera(args)

            # Capturing Camera footage
            for frame in pi_cam.cam.capture_continuous(raw_capture, format="bgr", use_video_port=True):
                try:
                    # assign Captured frame
                    captured_frame_array = frame.array

                    #streamManager.send_image(captured_frame_array)
                    # convert captured frame array to gray
                    gray_captured_frame = cv2.cvtColor(captured_frame_array, cv2.COLOR_BGR2GRAY)
                    # detect faces on frame
                    faces = face_cascade.detectMultiScale(gray_captured_frame, 1.2, 5)
                    print("Detected Faces: {0:d}".format(len(faces)))
                    if last_face_count != len(faces):
                        last_face_count = len(faces)
                        apiManager.post_face_count(len(faces))
                except Exception as e:
                    print("Face Recognition Error" + str(e))
                    raw_capture.truncate(0)
                    continue

                # Get Face Positions
                try:
                    # only one face was recognized
                    if len(faces) == 1:
                        # iterate through faces and get its dimensions
                        for (x, y, width, height) in faces:
                            # draw rectangle around face
                            cv2.rectangle(captured_frame_array, (x, y), (x + width, y + height), (255, 255, 0), 2)
                            print(
                                "Face Position: x = {0:d}, y = {1:d}, width = {2:d}, height = {3:d}".format(x, y, width,
                                                                                                            height))
                            apiManager.post_face(int(x), int(y), int(width), int(height))
                    else:
                        print("Detected {0:d} Faces".format(len(faces)))
                except Exception as e:
                    print(f"Error getting face position {str(e)}")
                    raw_capture.truncate(0)

                # send image per udp
                streamManager.send_image(captured_frame_array)
                # display video for test purposes
                #cv2.imshow("Frame", captured_frame_array)
                #cv2.waitKey(1) & 0xFF
                raw_capture.truncate(0)
    except KeyboardInterrupt:
        print("closing Program after user interrupt")
        pass


if __name__ == "__main__":
    main()
