import React from 'react'
import ReactFileReader from 'react-file-reader'
import "./HomePage.css"
import {Button} from '@mui/material'
function HomePage() {
  return (
    <div className='Container'>
        <div className='SubContainer'>
            <div className='Box1'></div>
            <div className='Box2'>
                <div className="data-flex">
                    <div className="UserID">UserID</div>
                    <div className="UserName">UserName</div>
                    <div className="EmailID">EmailID</div>
                    <div className="MobileNumber">MobileNumber</div>
                    <div className="Age">Age</div>
                    <div className="Salary">Salary</div>
                    <div className='Delete'></div>
                </div>
            </div>
            <div className='Input-Container'>
                <div className='Flex-Container'>
                   <div className='Header'>Excel and Csv Bulk Data Uploader</div>
                   <div className='Sub-flex-Container'>
                    <div className='FileName'></div>
                    <div className='UploadButton'>
                    <ReactFileReader fileTypes={".xlsx,.csv"} className="Upload">
                    <Button variant ="contained" color="primary">Submit</Button>
                    </ReactFileReader>
                   </div>
                   </div>
                </div>
            <div className='flex-Button'>
             <Button variant ="contained" color="secondary">Upload</Button>
            </div>
            </div>
        </div>
      
    </div>
  )
}

export default HomePage
