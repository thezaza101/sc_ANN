library(tidyverse)
library(plotly)


data <- read_csv("cancer.txtOut.csv")
scaleddata <- scale(data)
scaleddata <- data.frame(scaleddata)

data$MaxTest = scaleddata$MaxTest

data <- data %>%
  mutate(size=MaxTest==100) %>%
  mutate(size=replace(1, size, 0))

p <- plot_ly(data, x = ~Epochs, y = ~HiddenLayers, z = ~eta, color = ~MaxTest, size=1) %>%
  add_markers() %>%
  layout(scene = list(xaxis = list(title = 'Epochs'),
                      yaxis = list(title = 'HiddenLayers'),
                      zaxis = list(title = 'eta')))
