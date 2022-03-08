# install azure clr
# login azure
az login
# login acr
az acr login --name mymagiconion
# add tag to image
# docker tag <image> <acr_server_name>/<namespace>/<app>
docker tag grpcserver-image mymagiconion.azurecr.io/test/grpcserver
docker push mymagiconion.azurecr.io/test/grpcserver
# if you crate container from acr, acr accesskey must be enable
# see below
# https://docs.microsoft.com/en-us/azure/container-registry/container-registry-authentication?tabs=azure-cli#admin-account
