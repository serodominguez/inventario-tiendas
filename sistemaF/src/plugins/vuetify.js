import Vue from 'vue'
import Vuetify from 'vuetify/lib'
import 'vuetify/src/stylus/app.styl'

Vue.use(Vuetify, {
  theme: {
    primary: '#FF1C2B',
    secondary: '#BF1520',
    accent: '#E61927',
    error: '#ff0000',
    info: '#BF1520',
    success: '#800E16',
    warning: '#FFC107'
  },
  customProperties: true,
  iconfont: 'md',
})
