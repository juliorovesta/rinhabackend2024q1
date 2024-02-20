FROM mcr.microsoft.com/devcontainers/dotnet:dev-8.0-jammy

ARG USERNAME=vscode
ARG USER_UID=1000
# ARG USER_UID=100000
ARG USER_GID=$USER_UID
ARG USER_HOME=/home/$USERNAME

RUN groupmod --gid $USER_GID $USERNAME \
    && usermod --uid $USER_UID --gid $USER_GID $USERNAME \
    && chown -R $USER_UID:$USER_GID $USER_HOME \
    && apt-get update \
    && apt-get install -y sudo \
    && echo $USERNAME ALL=\(root\) NOPASSWD:ALL > /etc/sudoers.d/$USERNAME \
    && chmod 0440 /etc/sudoers.d/$USERNAME

# installing JDK
ARG JAVA_VERSION=21.0.2.13-1

RUN set -eux \
    && apt-get update \
    && apt-get install -y --no-install-recommends \
    curl ca-certificates gnupg software-properties-common fontconfig java-common \
    && curl -fL https://apt.corretto.aws/corretto.key | apt-key add - \
    && add-apt-repository 'deb https://apt.corretto.aws stable main' \
    && mkdir -p /usr/share/man/man1 || true \
    && apt-get update \
    && apt-get install -y java-21-amazon-corretto-jdk=1:$JAVA_VERSION

ENV LANG=C.UTF-8
ENV JAVA_HOME=/usr/lib/jvm/java-21-amazon-corretto

# installing Gatling
ARG GATLING_VERSION=3.10.3
ENV GATLING_HOME=$USER_HOME/gatling

RUN mkdir -p $GATLING_HOME && mkdir -p /tmp/downloads && \
    chmod a+rwx $GATLING_HOME && \
    curl -sf -o /tmp/downloads/gatling-$GATLING_VERSION.zip \
    -L https://repo1.maven.org/maven2/io/gatling/highcharts/gatling-charts-highcharts-bundle/$GATLING_VERSION/gatling-charts-highcharts-bundle-$GATLING_VERSION-bundle.zip && \
    mkdir -p /tmp/archive && cd /tmp/archive && \
    unzip /tmp/downloads/gatling-$GATLING_VERSION.zip && \
    mv /tmp/archive/gatling-charts-highcharts-bundle-$GATLING_VERSION/* $GATLING_HOME/

VOLUME ["$GATLING_HOME/conf","$GATLING_HOME/results","$GATLING_HOME/user-files"]

# set paths
ENV PATH=$JAVA_HOME/bin:$GATLING_HOME/bin:$PATH

# ## Non root user
# ARG NONROOT_USER=$USERNAME

# # Default to root only access to the Docker socket, set up non-root init script
# RUN touch /var/run/docker-host.sock \
#     && ln -s /var/run/docker-host.sock /var/run/docker.sock \
#     && apt-get update \
#     && apt-get -y install socat

# # Create docker-init.sh to spin up socat
# RUN echo "#!/bin/sh\n\
#     sudoIf() { if [ \"\$(id -u)\" -ne 0 ]; then sudo \"\$@\"; else \"\$@\"; fi }\n\
#     sudoIf rm -rf /var/run/docker.sock\n\
#     ((sudoIf socat UNIX-LISTEN:/var/run/docker.sock,fork,mode=660,user=${NONROOT_USER} UNIX-CONNECT:/var/run/docker-host.sock) 2>&1 >> /tmp/vscr-docker-from-docker.log) & > /dev/null\n\
#     \"\$@\"" >> /usr/local/share/docker-init.sh \
#     && chmod +x /usr/local/share/docker-init.sh

# Clean up
RUN apt-get autoremove -y \
    && apt-get clean -y \
    && apt-get purge -y --auto-remove -o APT::AutoRemove::RecommendsImportant=false \
    curl gnupg software-properties-common \
    && rm -rf /var/lib/apt/lists/* /root/.gnupg /tmp/*

# # VS Code overrides ENTRYPOINT and CMD when executing `docker run` by default.
# # Setting the ENTRYPOINT to docker-init.sh will configure non-root access to
# # the Docker socket if "overrideCommand": false is set in devcontainer.json.
# # The script will also execute CMD if you need to alter startup behaviors.
# ENTRYPOINT [ "/usr/local/share/docker-init.sh" ]
# CMD [ "sleep", "infinity" ]
