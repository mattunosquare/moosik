FROM node:16 as node-stage
WORKDIR /frontend
COPY package*.json /frontend/
RUN npm install
COPY . /frontend/
ARG API_URL
ENV REACT_APP_API_URL=${API_URL}
RUN npm run build 
FROM nginx:1.15.8-alpine
COPY --from=node-stage /frontend/build /usr/share/nginx/html
COPY --from=node-stage /frontend/nginx.conf /etc/nginx/conf.d/default.conf