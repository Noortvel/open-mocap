# open-mocap

# Prepare video before upload
```
$ ./ffmpeg -i <any_video> -c copy -movflags faststart <out_video>
```
# Prepare example
```
.\Binary\ffmpeg.exe -i .\ExamplesVideos\movie_trim.mp4 -c copy -movflags faststart .\ExamplesVideos\stream_ready.mp4
```

# Build and run
```
$ cd ./OpenMocap
$ docker-compose build
$ docker-compose up
```
