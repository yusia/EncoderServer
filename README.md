### How to run
docker build --pull --rm -f "Encoder\Dockerfile" -t base64-converter "Encoder" 
docker run --rm -p 5000:5000 -p 5001:5001 -e ASPNETCORE_HTTP_PORT=http://+:5001 -e ASPNETCORE_URLS=http://+:5000 base64-converter
