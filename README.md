# Headless Chrome using selenium/zealinium

install zalenium container

  ## Pull Zalenium
    docker pull dosel/zalenium
    
    # Run it!
    docker run --rm -ti --name zalenium -p 4444:4444 \
      -e PULL_SELENIUM_IMAGE=true \
      -v /var/run/docker.sock:/var/run/docker.sock \
      -v /tmp/videos:/home/seluser/videos \
      --privileged dosel/zalenium start
      
    # Point your tests to http://localhost:4444/wd/hub and run them

    # Stop
    docker stop zalenium


https://opensource.zalando.com/zalenium/


hit this url https://localhost:5001/api/values

monitor the live preview here
http://localhost:4444/grid/admin/live