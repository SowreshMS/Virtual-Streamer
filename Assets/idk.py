import os
import time
from google.cloud import texttospeech_v1beta1 as texttospeech
import threading
import pygame
import keyboard

os.environ['GOOGLE_APPLICATION_CREDENTIALS'] = r"C:\Users\Spher\OneDrive\Desktop\CS\AI\Kuebiko\AI-Virtual_Streamer\polynomial-land-400201-c0247bbd8ec8.json"

client = texttospeech.TextToSpeechClient()

def captions(mark_array, response):
    count = 0
    current = 0
    try: 
        for i in range(len(response.timepoints)):
            count += 1
            current += 1
            with open(r"C:\Users\Spher\Virtual-Streamer\Assets\speaker.txt", "a", encoding="utf-8") as out:
                out.write(mark_array[int(response.timepoints[i].mark_name)] + " ")
            if i != len(response.timepoints) - 1:
                total_time = response.timepoints[i + 1].time_seconds
                time.sleep(total_time - response.timepoints[i].time_seconds)
            if current == 25:
                    open(r"C:\Users\Spher\Virtual-Streamer\Assets\speaker.txt", 'w', encoding="utf-8").close()
                    current = 0
                    count = 0
            elif count % 7 == 0:
                with open(r"C:\Users\Spher\Virtual-Streamer\Assets\speaker.txt", "a", encoding="utf-8") as out:
                    out.write("\n")
        time.sleep(2)
        open(r"C:\Users\Spher\Virtual-Streamer\Assets\speaker.txt", 'w').close()
    except:
        pass

# Function to generate and play audio from binary audio data
def text_to_speech_and_play(audio_content):
    # Save the audio content to a file
    with open("output2.mp3", "wb") as out:
        out.write(audio_content)

    # Initialize pygame mixer
    pygame.mixer.init()

    # Load and play the audio file
    pygame.mixer.music.load("output2.mp3")
    pygame.mixer.music.play()

    # Wait for the audio to finish playing
    while pygame.mixer.music.get_busy():
        pygame.time.Clock().tick(10)

    # Quit the mixer
    pygame.mixer.quit()


def responding(response):
    # response = message.content + "? " + response
    ssml_text = '<speak>'
    response_counter = 0
    mark_array = []
    for s in response.split(' '):
        ssml_text += f'<mark name="{response_counter}"/>{s}'
        mark_array.append(s)
        response_counter += 1
    ssml_text += '</speak>'

    input_text = texttospeech.SynthesisInput(ssml=ssml_text)

    voice = texttospeech.VoiceSelectionParams(
        language_code="en-US",
        name="en-US-Neural2-F",
        ssml_gender=texttospeech.SsmlVoiceGender.FEMALE,
    )

    audio_config = texttospeech.AudioConfig(
        audio_encoding=texttospeech.AudioEncoding.MP3,
    )

    response = client.synthesize_speech(
        request={"input": input_text, "voice": voice, "audio_config": audio_config, "enable_time_pointing": ["SSML_MARK"]}
    )

    time_thread = threading.Thread(target=text_to_speech_and_play, args=(response.audio_content,))
    time2_thread = threading.Thread(target=captions, args=(mark_array, response))


    time2_thread.start()
    time_thread.start()

def open_file(filepath):
    with open(filepath, 'r', encoding='utf-8') as infile:
        return infile.read()
    


while True:
    if keyboard.is_pressed("q"):
        responding("Hi, welcome.")
    elif keyboard.is_pressed("w"):
        responding("I am Vivi, a streamer from the year 2050, where the real world and the virtual world collide into one.")
    elif keyboard.is_pressed("e"):
        responding("I am doing good, how about you.")
    elif keyboard.is_pressed("r"):
        responding("Sure, I would love to play flappy bird")
    elif keyboard.is_pressed("t"):
        responding("hi")
    else:
        continue

